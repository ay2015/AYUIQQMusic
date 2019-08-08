using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;
using System.Collections;
using Ay.Framework.WPF.Controls;
using Ay.Framework.WPF;
using System.Windows.Input;
using System.IO;
using System.Windows.Resources;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;
using Meta.Vlc.Wpf;
using Mp3Lib;
using System.Windows.Media;
using Ay.Framework.WPF.FuncFactory.Funcs;

namespace AYQQMusic.Models
{

    public class MusicMainWindowModel : AyPropertyChanged, IAyDragSource
    {

        private bool worldPlayStatus = false;

        public bool WorldPlayStatus
        {
            get { return worldPlayStatus; }
            set
            {
                if (worldPlayStatus != value)
                {
                    worldPlayStatus = value;
                    this.OnPropertyChanged("WorldPlayStatus");
                }

            }
        }

        private bool worldLoveStatus = false;

        public bool WorldLoveStatus
        {
            get { return worldLoveStatus; }
            set
            {
                if (worldLoveStatus != value)
                {
                    worldLoveStatus = value;
                    this.OnPropertyChanged("WorldLoveStatus");
                }

            }
        }


        private float currentPlayerPosition = 0.00F;

        public float CurrentPlayerPosition
        {
            get { return currentPlayerPosition; }
            set
            {
                if (currentPlayerPosition != value)
                {
                    currentPlayerPosition = value;
                    OnPropertyChanged("CurrentPlayerPosition");
                }
            }
        }

        private string currentTime = "00:00";

        public string CurrentTime
        {
            get { return currentTime; }
            set
            {
                if (currentTime != value)
                {
                    currentTime = value;
                    this.OnPropertyChanged("CurrentTime");
                }
            }
        }

        private bool progressEnbaled;

        public bool ProgressEnbaled
        {
            get { return progressEnbaled; }
            set
            {
                if (progressEnbaled != value)
                {
                    progressEnbaled = value;
                    this.OnPropertyChanged("ProgressEnbaled");
                }
            }
        }
        private string singer;

        public string Singer
        {
            get { return singer; }
            set
            {
                if (singer != value)
                {
                    singer = value;
                    this.OnPropertyChanged("Singer");
                }
            }
        }

        private string totalTime = "00:00";

        public string TotalTime
        {
            get { return totalTime; }
            set
            {
                if (totalTime != value)
                {
                    totalTime = value;
                    this.OnPropertyChanged("TotalTime");
                }
            }
        }

        private string geName = "AYUI音乐，听我想听的歌";

        public string GeName
        {
            get { return geName; }
            set
            {
                if (geName != value)
                {
                    geName = value;
                    this.OnPropertyChanged("GeName");
                }
            }
        }


        public static Cursor curDragInsert;
        public static Cursor curDrag;
        public static Cursor curDragNone;

        public ObservableCollection<PlayListModel> MyCreatePlayLists { get; set; }
        public PlayListModel PlayQueue { get; set; }
        public MusicMainWindowModel()
        {
            MyCreatePlayLists = new ObservableCollection<PlayListModel>();

            MyCreatePlayLists.Add(new PlayListModel
            {
                PlayListName = "我喜欢",
                Items = new ObservableCollection<PlayListItemModel>
                {

                }
            });

            PlayQueue = new PlayListModel
            {
                PlayListName = "播放对列",
                Items = new ObservableCollection<PlayListItemModel>
                {

                }
            };


            curDragInsert = new Cursor(new System.IO.MemoryStream(AYQQMusic.Properties.Resources._1064));
            curDrag = new Cursor(new System.IO.MemoryStream(AYQQMusic.Properties.Resources._1066));
            curDragNone = new Cursor(new System.IO.MemoryStream(AYQQMusic.Properties.Resources._1069));
        }

        public static GiveFeedbackEventHandler handler22;
        public void StartDrag(AyDragInfo dragInfo)
        {
            int itemCount = dragInfo.SourceItems.Cast<object>().Count();

            if (itemCount == 1)
            {
                dragInfo.Data = dragInfo.SourceItems.Cast<object>().First();
            }
            else if (itemCount > 1)
            {
                dragInfo.Data = TypeUtilities.CreateDynamicallyTypedList(dragInfo.SourceItems);
            }


            dragInfo.Effects = (dragInfo.Data != null) ?
                DragDropEffects.Copy | DragDropEffects.Move :
                DragDropEffects.None;
            if (handler22 == null)
            {
                handler22 = new GiveFeedbackEventHandler(DragSource_GiveFeedback);
            }

            dragInfo.VisualSource.GiveFeedback += handler22;

        }
        void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            Mouse.SetCursor(curDrag);
            e.UseDefaultCursors = false;
            e.Handled = true;
        }

        public void DoubleClickItem(AyDragInfo dragInfo)
        {
            PlayListItemModel music = dragInfo.SourceItem as PlayListItemModel;
            SetPlayLock();
            PlayAyMusic(music);
        }
        internal bool sliderProgressLock = false;
        internal static DispatcherTimer currentPlayTimer = new DispatcherTimer(DispatcherPriority.Render);
        internal static PlayListItemModel lastMusic;
        static ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
        internal static void PlayAyMusic(PlayListItemModel music)
        {
            if (System.IO.File.Exists(music.SongPath))
            {
                if (currentPlayTimer != null)
                {
                    currentPlayTimer.Stop();
                    currentPlayTimer = null;
                }
                MainWindow.curr.ProgressEnbaled = true;
                if (lastMusic != null)
                {
                    MainWindow.CurrentPlayer.Stop();
                    MainWindow.CurrentPlayer.Dispose();
                }
                
                if (music.ExtName == ".mp3")
                {
                    Mp3File mp3 = new Mp3File(music.SongPath);
                    if (mp3.TagHandler.Picture != null)
                    {
                        MemoryStream stream = new MemoryStream(AyFuncFactory.GetFunc<AyFuncBitmapWithWpf>().ImageToBytes(mp3.TagHandler.Picture, System.Drawing.Imaging.ImageFormat.Jpeg));
                        string filename = AyFuncFactory.GetFunc<AyFuncSecrity>().GetMD5Result(mp3.TagHandler.Song + mp3.TagHandler.Artist.ToObjectString());
                        filename = ExtendUtils.GetDATA_TEMP_ALBUM_PATH() + "\\" + filename + ".jpg";
                        var dsd = imageSourceConverter.ConvertFrom(stream);
                        if (!File.Exists(filename))
                        {
                            System.Drawing.Image bm = System.Drawing.Image.FromStream(stream, true);
                            bm.Save(filename);

                        }
                        if (dsd == null)
                        {
                            MainWindow.SetAlbumImage(null, filename);
                        }
                        else
                        {
                            MainWindow.SetAlbumImage(dsd as System.Windows.Media.Imaging.BitmapFrame, filename);
                        }


                    }
                    else
                    {
                        MainWindow.SetAlbumImage(null);
                    }


                    MainWindow.curr.Singer = "- " + mp3.TagHandler.Artist;
                    mp3 = null;
                    AyExtension.MemoryGC();

                }
                else
                {
                    MainWindow.curr.Singer = "- 未知AY";
                }

                MainWindow.CurrentPlayer = new VlcPlayer();
                MainWindow.CurrentPlayer.Initialize(@"Contents\LibVlc", new string[] { "-I", "--dummy", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file" });
                MainWindow.CurrentPlayer.EndBehavior = EndBehavior.Repeat;
                MainWindow.CurrentPlayer.LoadMedia(music.SongPath);
                MainWindow.CurrentPlayer.Play();

                MainWindow.CurSongGuid = music.SongGuid;
                music.PlayStatus = true;
                MainWindow.curr.WorldPlayStatus = true;
                MainWindow.curr.GeName = music.SongNameWithoutExt;
                if (lastMusic != null && lastMusic.SongGuid != music.SongGuid)
                {
                    lastMusic.PlayStatus = false;
                }
                lastMusic = music;
                MainWindow.curr.WorldLoveStatus = music.LoveStatus;
            
              
                currentPlayTimer = AyTime.setInterval(100, () =>
                {
                    var totalDuration = MainWindow.CurrentPlayer.GetDuration();
                    if (totalDuration.Hours > 0)
                    {
                        MainWindow.curr.TotalTime = string.Format("{0:00}:{1:00}:{2:00}", totalDuration.Hours, totalDuration.Minutes, totalDuration.Seconds);
                    }
                    else if (totalDuration.Hours == 0)
                    {
                        MainWindow.curr.TotalTime = string.Format("{0:00}:{1:00}", totalDuration.Minutes, totalDuration.Seconds);
                    }
                    if (MainWindow.CurrentPlayer != null && (MainWindow.CurrentPlayer.State == Meta.Vlc.Interop.Media.MediaState.Paused || MainWindow.CurrentPlayer.State == Meta.Vlc.Interop.Media.MediaState.Stopped))
                    {
                        currentPlayTimer.Stop();
                    }
                    var dd = MainWindow.CurrentPlayer.GetPlayTime();
                    MainWindow.curr.CurrentTime = string.Format("{0:00}:{1:00}:{2:00}", dd.Hours, dd.Minutes, dd.Seconds);
                    if (dd.Hours > 0)
                    {
                        MainWindow.curr.CurrentTime = string.Format("{0:00}:{1:00}:{2:00}", dd.Hours, dd.Minutes, dd.Seconds);
                    }
                    else if (dd.Hours == 0)
                    {
                        MainWindow.curr.CurrentTime = string.Format("{0:00}:{1:00}", dd.Minutes, dd.Seconds);
                    }
                    if (!MainWindow.curr.sliderProgressLock)
                    {
                        MainWindow.curr.CurrentPlayerPosition = MainWindow.CurrentPlayer.Position;
                    }

                });


            }
            else
            {
                AyMessageBox.ShowInformation("音乐文件不存在！");
            }
        }
        /// <summary>
        /// 是否锁住
        /// </summary>
        internal static bool WORLDPLAYLOCK = false;
        internal void SetPlayLock()
        {
            WORLDPLAYLOCK = true;
            AyTime.setTimeout(300, () =>
            {
                WORLDPLAYLOCK = false;
            });
        }

        /// <summary>
        /// 是否锁住like
        /// </summary>
        internal static bool WORLDLOVELOCK = false;
        internal void SetLoveLock()
        {
            WORLDLOVELOCK = true;
            AyTime.setTimeout(300, () =>
            {
                WORLDLOVELOCK = false;
            });
        }
    }
}
