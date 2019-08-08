using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AYQQMusic.Models
{
    [Serializable]
    public class Mp3SerializeHelper
    {
        /// <summary>
        /// 注册--保存
        /// </summary>
        public static void SaveCurrentPlay(List<PlayListItemModelSerial> data, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                //二进制
                BinaryFormatter bf = new BinaryFormatter();
                //序列化
                bf.Serialize(fs, data);
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public static List<PlayListItemModelSerial> LoadCurrentPlay(string filename)
        {
            List<PlayListItemModelSerial> ss = null;

            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    //二进制格式化
                    BinaryFormatter bf = new BinaryFormatter();
                    //反序列化
                    ss = (List<PlayListItemModelSerial>)bf.Deserialize(fs);
                }
            }
            return ss;
        }
    }
        [Serializable]
    public class Mp3SerializeHelper<T>
    {

        #region 保存与读取
        /// <summary>
        /// 注册--保存
        /// </summary>
        public static void Save(Dictionary<string, T> data,string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                //二进制
                BinaryFormatter bf = new BinaryFormatter();
                //序列化
                bf.Serialize(fs, data);
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, T> Load(string filename)
        {
            Dictionary<string, T> ss = null;

            if (File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    //二进制格式化
                    BinaryFormatter bf = new BinaryFormatter();
                    //反序列化
                    ss = (Dictionary<string, T>)bf.Deserialize(fs);
                }
            }
            return ss;
        }

       
        #endregion

    }

   
}
