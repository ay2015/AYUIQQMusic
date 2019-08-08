using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace AYQQMusic.Models.Utility
{

    public class AyLyricHelper
    {
        //歌词Id获取地址
        private static readonly string SearchPath = "http://ttlrcct2.qianqian.com/dll/lyricsvr.dll?sh?Artist={0}&Title={1}&Flags=0";

        //根据artist和title获取歌词信息
        public static LrcInfo[] GetLrcList(string artist, string title, string filepath)
        {
            string artistHex = GetHexString(artist, Encoding.Unicode);
            string titleHex = GetHexString(title, Encoding.Unicode);

            string resultUrl = string.Format(SearchPath, artistHex, titleHex);

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(resultUrl);

                XmlNodeList nodelist = doc.SelectNodes("/result/lrc");
                List<LrcInfo> lrclist = new List<LrcInfo>();
                foreach (XmlNode node in nodelist)
                {
                    XmlElement element = (XmlElement)node;
                    string artistItem = element.GetAttribute("artist");
                    string titleItem = element.GetAttribute("title");
                    string idItem = element.GetAttribute("id");
                    lrclist.Add(new LrcInfo(idItem, titleItem, artistItem, filepath));
                }
                return lrclist.ToArray();
            }
            catch (XmlException)
            {
                return null;
            }
        }

        //把字符串转换为十六进制
        public static string GetHexString(string str, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = encoding.GetBytes(str);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("X").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }

    public class LrcInfo
    {
        //歌词下载地址
        private static readonly string DownloadPath = "http://ttlrcct2.qianqian.com/dll/lyricsvr.dll?dl?Id={0}&Code={1}";

        public string FilePath { get; set; }
        public string Id { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string LrcUri { get; set; }

        public LrcInfo(string id, string title, string artist, string filepath)
        {
            this.FilePath = filepath;
            this.Id = id.Trim();
            this.Title = title;
            this.Artist = artist;
            //算出歌词的下载地址
            this.LrcUri = string.Format(DownloadPath, Id, CreateQianQianCode());
        }
        public bool DownloadLrc()
        {
            string file = FilePath;
            string directory = Path.GetDirectoryName(file) + "\\Lrc\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string filepath = directory + Path.GetFileNameWithoutExtension(file) + ".lrc";
            WebRequest request = WebRequest.Create(LrcUri);

            StringBuilder sb = new StringBuilder();
            try
            {
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    using (StreamWriter sw = new StreamWriter(filepath, false, Encoding.UTF8))
                    {
                        sw.Write(sr.ReadToEnd());
                    }
                }
                return true;
            }
            catch (WebException)
            {

            }
            return false;
        }


        private string CreateQianQianCode()
        {
            int lrcId = Convert.ToInt32(Id);
            string qqHexStr = AyLyricHelper.GetHexString(Artist + Title, Encoding.UTF8);
            int length = qqHexStr.Length / 2;
            int[] song = new int[length];
            for (int i = 0; i < length; i++)
            {
                song[i] = int.Parse(qqHexStr.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            int t1 = 0, t2 = 0, t3 = 0;
            t1 = (lrcId & 0x0000FF00) >> 8;
            if ((lrcId & 0x00FF0000) == 0)
            {
                t3 = 0x000000FF & ~t1;
            }
            else
            {
                t3 = 0x000000FF & ((lrcId & 0x00FF0000) >> 16);
            }

            t3 = t3 | ((0x000000FF & lrcId) << 8);
            t3 = t3 << 8;
            t3 = t3 | (0x000000FF & t1);
            t3 = t3 << 8;
            if ((lrcId & 0xFF000000) == 0)
            {
                t3 = t3 | (0x000000FF & (~lrcId));
            }
            else
            {
                t3 = t3 | (0x000000FF & (lrcId >> 24));
            }

            int j = length - 1;
            while (j >= 0)
            {
                int c = song[j];
                if (c >= 0x80) c = c - 0x100;

                t1 = (int)((c + t2) & 0x00000000FFFFFFFF);
                t2 = (int)((t2 << (j % 2 + 4)) & 0x00000000FFFFFFFF);
                t2 = (int)((t1 + t2) & 0x00000000FFFFFFFF);
                j -= 1;
            }
            j = 0;
            t1 = 0;
            while (j <= length - 1)
            {
                int c = song[j];
                if (c >= 128) c = c - 256;
                int t4 = (int)((c + t1) & 0x00000000FFFFFFFF);
                t1 = (int)((t1 << (j % 2 + 3)) & 0x00000000FFFFFFFF);
                t1 = (int)((t1 + t4) & 0x00000000FFFFFFFF);
                j += 1;
            }

            int t5 = (int)Conv(t2 ^ t3);
            t5 = (int)Conv(t5 + (t1 | lrcId));
            t5 = (int)Conv(t5 * (t1 | t3));
            t5 = (int)Conv(t5 * (t2 ^ lrcId));

            long t6 = (long)t5;
            if (t6 > 2147483648)
                t5 = (int)(t6 - 4294967296);
            return t5.ToString();
        }
        private long Conv(int i)
        {
            long r = i % 4294967296;
            if (i >= 0 && r > 2147483648)
                r = r - 4294967296;

            if (i < 0 && r < 2147483648)
                r = r + 4294967296;
            return r;
        }

    }


    //LrcInfo[] infos = LyricsHelper.GetLrcList("王菲", "匆匆那年", @"C:\王菲 - 匆匆那年.mp3");
    //        if (infos[0].DownloadLrc())
    //        {
    //            Console.WriteLine("下载成功");
    //        }
}
