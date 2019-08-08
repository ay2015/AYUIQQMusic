using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

/// <summary>
/// ayui 音乐播放器的 主界面列表model
/// 合肥 杨洋 875556003
/// </summary>
namespace AYQQMusic.Models
{
    [Serializable]
    public class PlayListItemModelSerial 
    {
        private string extname;

        public string ExtName
        {
            get { return extname; }
            set { extname = value; }
        }
        
        public string SongGuid { get; set; }

        private int songId;

        public int SongId
        {
            get { return songId; }
            set
            {
                if (songId != value)
                {
                    songId = value;
 
                }

            }
        }
        private string songNameWithoutExt;

        public string SongNameWithoutExt
        {
            get { return songNameWithoutExt; }
            set
            {
                if (songNameWithoutExt != value)
                {
                    songNameWithoutExt = value;
                  
                }
            }
        }


        private string songName;

        public string SongName
        {
            get { return songName; }
            set
            {
                if (songName != value)
                {
                    songName = value;
   
                }


            }
        }



        private string songPath;

        public string SongPath
        {
            get { return songPath; }
            set
            {
                if (songPath != value)
                {
                    songPath = value;
              
                }


            }
        }

        private bool lrcNeedDown;

        public bool LrcNeedDown
        {
            get { return lrcNeedDown; }
            set
            {
                if (lrcNeedDown != value)
                {
                    lrcNeedDown = value;
            
                }
            }
        }


        private string lrcPath;

        public string LrcPath
        {
            get { return lrcPath; }
            set
            {
                if (lrcPath != value)
                {
                    lrcPath = value;
           
                }
            }
        }

        private string lrcFileName;

        public string LrcFileName
        {
            get { return lrcFileName; }
            set
            {
                if (lrcFileName != value)
                {
                    lrcFileName = value;
                 
                }
            }
        }


        private string songSinger;

        public string SongSinger
        {
            get { return songSinger; }
            set
            {
                if (songSinger != value)
                {
                    songSinger = value;
                  
                }
            }
        }
        private string songSingerId;

        public string SongSingerId
        {
            get { return songSingerId; }
            set
            {
                if (songSingerId != value)
                {
                    songSingerId = value;
                  
                }
            }
        }



        private bool playStatus = false;

        public bool PlayStatus
        {
            get { return playStatus; }
            set
            {
                if (playStatus != value)
                {
                    playStatus = value;
         
                }
            }
        }

        private bool loveStatus = false;

        public bool LoveStatus
        {
            get { return loveStatus; }
            set
            {
                if (loveStatus != value)
                {
                    loveStatus = value;
                
                }
            }
        }


        private DateTime allPlayTime = new DateTime(1991, 4, 4, 0, 0, 0);

        public DateTime AllPlayTime
        {
            get { return allPlayTime; }
            set
            {
                if (allPlayTime != value)
                {
                    allPlayTime = value;
             
                }
            }
        }



        public string AllTime
        {
            get
            {
                if (AllPlayTime.Hour > 0)
                {
                    return AllPlayTime.ToString("HH:mm:ss");
                }
                else
                {
                    return AllPlayTime.ToString("mm:ss");
                }
            }
        }


        public string CurrentTime
        {
            get
            {
                if (CurrentPlayTime.Hour > 0)
                {
                    return CurrentPlayTime.ToString("HH:mm:ss");
                }
                else
                {
                    return CurrentPlayTime.ToString("mm:ss");
                }
            }
        }




        private DateTime currentPlayTime = new DateTime(1991, 4, 4, 0, 0, 0);

        public DateTime CurrentPlayTime
        {
            get { return currentPlayTime; }
            set
            {
                if (currentPlayTime != value)
                {
                    currentPlayTime = value;
         
                }
            }
        }


        private bool biaoZhunYinZhi;

        public bool BiaoZhunYinZhi
        {
            get { return biaoZhunYinZhi; }
            set
            {

                if (biaoZhunYinZhi != value)
                {
                    biaoZhunYinZhi = value;
               
                }
            }
        }

        private bool hqYinZhi;

        public bool HqYinZhi
        {
            get { return hqYinZhi; }
            set
            {
                if (hqYinZhi != value)
                {
                    hqYinZhi = value;
                   
                }
            }
        }

        private bool sqAPE;

        public bool SqAPE
        {
            get { return sqAPE; }
            set
            {
                if (sqAPE != value)
                {
                    sqAPE = value;
                  
                }
            }
        }

        private bool sqFLAC;

        public bool SqFLAC
        {
            get { return sqFLAC; }
            set
            {
                if (sqFLAC != value)
                {
                    sqFLAC = value;
               
                }
            }
        }

        private bool fLAC51;

        public bool FLAC51
        {
            get { return fLAC51; }
            set
            {
                if (fLAC51 != value)
                {
                    fLAC51 = value;
               
                }
            }
        }


    }
    [Serializable]
    public class PlayListModelSerial 
    {
        private string icon;

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }


        private bool canDelete = false;

        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }

        private string playListName;

        public string PlayListName
        {
            get { return playListName; }
            set
            {
                if (playListName != value)
                {
                    playListName = value;
                 
                }
            }
        }

        private List<PlayListItemModel> items;

        public List<PlayListItemModel> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                   
                }
            }
        }



    }
}
