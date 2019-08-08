using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Ay.Framework.WPF;
using Ay.Framework.WPF.Controls;
using AYQQMusic.Models;
using Meta.Vlc.Wpf;
using Mp3Lib;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Media;
using AYQQMusic.Controls;
using System.Windows.Media.Animation;


namespace AYQQMusic
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : AyWindow
    {
        public static MusicMainWindowModel curr;
        public static string CurSongGuid = "";

        public void LoadLocalData()
        {
            var loadCurrentData = Mp3SerializeHelper.LoadCurrentPlay(ExtendUtils.GetDATA_PATH() + ExtendUtils.CurrentPlayDbName);

            curr = this.DataContext as MusicMainWindowModel;
            if (loadCurrentData != null)
            {
                ObservableCollection<PlayListItemModel> ps = new ObservableCollection<PlayListItemModel>();
                foreach (var item in loadCurrentData)
                {
                    PlayListItemModel pim = new PlayListItemModel();
                    pim.SongGuid = item.SongGuid;
                    pim.SongId = item.SongId;
                    pim.SongName = item.SongName;
                    pim.SongNameWithoutExt = item.SongNameWithoutExt;
                    pim.SongPath = item.SongPath;
                    pim.SongSinger = item.SongSinger;
                    pim.SongSingerId = item.SongSingerId;
                    pim.AllPlayTime = item.AllPlayTime;
                    pim.LrcFileName = item.LrcFileName;
                    pim.LrcNeedDown = item.LrcNeedDown;
                    pim.ExtName = item.ExtName;
                    pim.LrcPath = item.LrcPath;
                    pim.LoveStatus = item.LoveStatus;
                    pim.PlayStatus = item.PlayStatus;
                    pim.CurrentPlayTime = item.CurrentPlayTime;
                    pim.BiaoZhunYinZhi = item.BiaoZhunYinZhi;
                    pim.HqYinZhi = item.HqYinZhi;
                    pim.SqAPE = item.SqAPE;
                    pim.SqFLAC = item.SqFLAC;
                    pim.FLAC51 = item.FLAC51;
                    ps.Add(pim);
                }

                curr.PlayQueue.Items = ps;
            }
            AyThread.Instance.InitDispatcher(this.Dispatcher);
        }
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
            string realPath = string.Format("pack://application:,,,/AYQQMusic;component/Contents/Images/Album/defaultSmallPng.png");
            Uri pathUri = new Uri(realPath, UriKind.RelativeOrAbsolute);
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage(pathUri);
            DefaultAlbumImage = bi;

            CurrentPlayer.Initialize(@"Contents\LibVlc", new string[] { "-I", "--dummy", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file" });
            CurrentPlayer.EndBehavior = EndBehavior.Nothing;
        }


        public string GetMusicLengthString(string path)
        {
            Mp3File mp3File = new Mp3File(path);
            TimeSpan ts = new TimeSpan(0, 0, (int)mp3File.Audio.Duration);
            int minuts = ts.Minutes;
            int second = ts.Seconds;
            mp3File = null;
            return minuts.ToString("d2") + ":" + second.ToString("d2");

        }

        #region 用于控制浏览器打开网页
        WebPageViewer cefWeb = null;
        //FormsWebBrowser web = null;
        #endregion
        public void NewWebUrl(string tmd)
        {
            this.layoutMain.Children.Clear();
            cefWeb = new WebPageViewer(tmd);
            this.layoutMain.Children.Add(cefWeb);
        }
        private void rdo1_Checked(object sender, RoutedEventArgs e)
        {
            AyNavRadioButton nrb1 = sender as AyNavRadioButton;
            if (nrb1 != null)
            {
                if (nrb1.Tag != null)
                {
                    var tmd = nrb1.Tag.ToString();
                    if (string.IsNullOrEmpty(tmd)) return;
                    if (tmd.IndexOf("http:/") == 0 || tmd.IndexOf("https:/") == 0 || tmd.IndexOf("file:/") == 0)
                    {
                        //if (web == null)
                        //{
                        //    web = new FormsWebBrowser(this.layoutMain, false); //或者直接this
                        //}
                        //web.WebBrowser.Navigate(new Uri(tmd));
                        NewWebUrl(tmd);
                    }
                    else
                    {
                        this.layoutMain.Children.Clear();
                        if (cefWeb != null)
                        {
                            cefWeb = null;
                        }
                        //if (web != null)
                        //{
                        //    web._formCloseLock = true;
                        //    web._form.Close();
                        //    web = null;
                        //}
                        Frame n = new Frame();
                        n.VerticalAlignment = VerticalAlignment.Stretch;
                        n.HorizontalAlignment = HorizontalAlignment.Stretch;
                        n.NavigationUIVisibility = NavigationUIVisibility.Hidden;
                        n.Source = new Uri(tmd, UriKind.Relative);
                        this.layoutMain.Children.Add(cefWeb);
                    }

                }
            }
        }

        Color c1 = SolidColorBrushConverter.ToColor("#FF2D2E30");
        Color c2 = SolidColorBrushConverter.ToColor("#002D2E30");
        ColorAnimationUsingKeyFrames ControlBarUpColorFrames = new ColorAnimationUsingKeyFrames();
        ColorAnimationUsingKeyFrames ControlBarDownColorFrames = new ColorAnimationUsingKeyFrames();
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MainWindow_Loaded;
            double canvasFromheight = 0;
            canvasFromheight = InitgeAlbum(canvasFromheight);
            localAlbum = albumButton;
            localAlbum.Icon = DefaultAlbumImage;
            base.MaxWindowMethodOverride += () =>
            {
                base.DoMaxOrReStoreWindow();

                if (this.WindowState == WindowState.Normal)
                {
                    tiaozhengwindow.Visibility = Visibility.Visible;
                    musicborder.BorderThickness = new Thickness(1, 1, 1, 0);
                }
                else
                {
                    tiaozhengwindow.Visibility = Visibility.Collapsed;
                    musicborder.BorderThickness = new Thickness(0);
                }
                UpdatePlayListLocation();
            };
            base.ResizeWindowInvokeMethod += () =>
            {
                UpdatePlayListLocation();
            };

            LoadLocalData();
            nav1.IsChecked = true;
            zhankaishouqi1.IsChecked = false;
            shoucanggedan.IsChecked = true;

            stb = swichyin;
            nav201.IsChecked = true;
            stbYinXiao = swichyinxiao;
            nav301.IsChecked = true;
            yinzhiPopup.Closed += YinzhiPopup_Closed;
            yinxiaoPopup.Closed += YinxiaoPopup_Closed;
            bofangduiliePopup.Closed += BofangduiliePopup_Closed;

            double mintime = 200;
            HeightKey = new DoubleAnimationUsingKeyFrames();
            HeightKey.Duration = new Duration(TimeSpan.FromMilliseconds(mintime));
            HeightKey.FillBehavior = FillBehavior.Stop;

            EasingDoubleKeyFrame HeightKey0 = new EasingDoubleKeyFrame(0.00, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(mintime)));
            HeightKey0.EasingFunction = easyOut;
            HeightKey.KeyFrames.Add(HeightKey0);
            HeightKey.Completed += HeightKey_Completed;



            EasingColorKeyFrame c2_0 = new EasingColorKeyFrame(c2, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(mintime)));
            ControlBarUpColorFrames.Duration = new Duration(TimeSpan.FromMilliseconds(mintime));
            ControlBarUpColorFrames.FillBehavior = FillBehavior.Stop;
            c2_0.EasingFunction = easyIn;
            ControlBarUpColorFrames.KeyFrames.Add(c2_0);
            ControlBarUpColorFrames.Completed += (a, ed) =>
            {
                childControlbarColor.Color = c2;
            };

            EasingColorKeyFrame c1_0 = new EasingColorKeyFrame(c1, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(mintime)));
            ControlBarDownColorFrames.Duration = new Duration(TimeSpan.FromMilliseconds(mintime));
            ControlBarDownColorFrames.FillBehavior = FillBehavior.Stop;
            c1_0.EasingFunction = easyOut;
            ControlBarDownColorFrames.KeyFrames.Add(c1_0);
            ControlBarDownColorFrames.Completed += (a, ed) =>
            {
                childControlbarColor.Color = c1;
            };
            AyExtension.SetAyWindowMouseLeftButtonCommonClick(musicWindowGrid);
            AyExtension.SetAyWindowMouseLeftButtonCommonClick(childControlbar);
           
        }
        public void Animation_ControlBarUp()
        {
            childControlbarColor.BeginAnimation(SolidColorBrush.ColorProperty, ControlBarUpColorFrames);
        }
        public void Animation_ControlBarDown()
        {
            childControlbarColor.BeginAnimation(SolidColorBrush.ColorProperty, ControlBarDownColorFrames);
        }

        DoubleAnimationUsingKeyFrames HeightKey;

        private void AddTaskBarButton()
        {
            //ThumbnailToolbarButton buttonFirst = new ThumbnailToolbarButton(
            //               Win7TaskbarDemo.Properties.Resources.First, "First Image");
            //buttonFirst.Enabled = true;
            //buttonFirst.Click += buttonFirst_Click;

            //ThumbnailToolbarButton buttonPrevious = new ThumbnailToolbarButton(
            //                       Win7TaskbarDemo.Properties.Resources.Previous, "Previous Image");
            //buttonPrevious.Enabled = true;
            //buttonPrevious.Click += buttonPrevious_Click;

            //ThumbnailToolbarButton buttonNext = new ThumbnailToolbarButton(
            //                       Win7TaskbarDemo.Properties.Resources.Next, "Next Image");
            //buttonPrevious.Enabled = true;
            //buttonNext.Click += buttonNext_Click;

            //ThumbnailToolbarButton buttonLast = new ThumbnailToolbarButton(
            //                       Win7TaskbarDemo.Properties.Resources.Last, "Last Image");
            //buttonPrevious.Enabled = true;
            //buttonLast.Click += buttonLast_Click;
        }
        private void UpdatePlayListLocation()
        {
            var offset = bofangduiliePopup.HorizontalOffset;
            bofangduiliePopup.HorizontalOffset = offset + 1;
            bofangduiliePopup.HorizontalOffset = offset;
        }

        private void BofangduiliePopup_Closed(object sender, EventArgs e)
        {
            bofangduilie.IsChecked = false;
        }

        private void YinxiaoPopup_Closed(object sender, EventArgs e)
        {
            stbYinXiao.IsChecked = false;
            yinXiaoContentImg.Source = null;
            yinxiaoSliderlist.Visibility = Visibility.Collapsed;
            huanraoSlider.Visibility = Visibility.Collapsed;
            diyinSlider.Visibility = Visibility.Collapsed;
            renshengSlider.Visibility = Visibility.Collapsed;
            xinchangSlider.Visibility = Visibility.Collapsed;
            chakanxiangqing.Visibility = Visibility.Collapsed;
            ruo.Visibility = Visibility.Collapsed;
            qiang.Visibility = Visibility.Collapsed;

        }

        private void YinzhiPopup_Closed(object sender, EventArgs e)
        {
            stb.IsChecked = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AyMessageBox.ShowInformation(searchmusic.Text);
        }

        private void prewPage_Click(object sender, RoutedEventArgs e)
        {
            //if (web != null)
            //{
            //    web.WebBrowser.GoBack();
            //}
            if (cefWeb.IsNotNull()) cefWeb.GoBack();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            //if (web != null)
            //{
            //    web.WebBrowser.GoForward();
            //}
            if (cefWeb.IsNotNull()) cefWeb.GoForward();
        }

        private void refreshPage_Click(object sender, RoutedEventArgs e)
        {
            //if (web != null)
            //{
            //    web.WebBrowser.Refresh();
            //}
            if (cefWeb.IsNotNull()) cefWeb.Refresh(true);
        }

        public bool openYinZhiPop = false;

        public bool openYinXiaoPop = false;

        public bool openbofangduiliePop = false;
        private void swichyin_Checked(object sender, RoutedEventArgs e)
        {

            if (yinzhi)
            {

                yinzhiPopup.IsOpen = true;
                openYinZhiPop = true;
            }
            yinzhi = true;
        }

        private void swichyin_Unchecked(object sender, RoutedEventArgs e)
        {

            if (yinzhi)
            {

                yinzhiPopup.IsOpen = false;
                openYinZhiPop = false;
            }
            yinzhi = true;
        }

        AyImageToggleButton stb;
        AyImageToggleButton stbYinXiao;

        bool yinxiao = true;
        bool yinzhi = true;

        private void swichyinxiao_Checked(object sender, RoutedEventArgs e)
        {
            if (yinxiao)
            {

                yinxiaoPopup.IsOpen = true;
                openYinXiaoPop = true;
            }
            yinxiao = true;

        }

        private void swichyinxiao_Unchecked(object sender, RoutedEventArgs e)
        {
            CloseYinXiao();
        }

        private void CloseYinXiao()
        {
            if (yinxiao)
            {

                yinxiaoPopup.IsOpen = false;
                openYinXiaoPop = false;
            }
            yinxiao = true;
        }


        private void xiazai_Unchecked(object sender, RoutedEventArgs e)
        {

        }


        private void xiazai_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void gengduo_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void gengduo_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void yinliang_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void yinliang_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void geci_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void geci_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void xunhuanfangshi_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void xunhuanfangshi_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        bool bfdl = true;
        private void bofangduilie_Checked(object sender, RoutedEventArgs e)
        {

            if (bfdl)
            {

                bofangduiliePopup.IsOpen = true;
                openbofangduiliePop = true;
            }
            bfdl = true;
        }

        private void bofangduilie_Unchecked(object sender, RoutedEventArgs e)
        {

            if (bfdl)
            {

                bofangduiliePopup.IsOpen = false;
                openbofangduiliePop = false;
            }
            bfdl = true;
        }


        private void chuangge_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chuangge_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void newplaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void zhankaishouqi1_Checked(object sender, RoutedEventArgs e)
        {
            if (zhankaishouqi1 != null && gedanStack != null)
            {
                gedanStack.Visibility = Visibility.Collapsed;
                zhankaishouqi1.ToolTip = "展开";
            }


        }

        private void zhankaishouqi1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (zhankaishouqi1 != null && gedanStack != null)
            {
                gedanStack.Visibility = Visibility.Visible;
                zhankaishouqi1.ToolTip = "收起";
            }
        }

        private void shoucanggedan_Unchecked(object sender, RoutedEventArgs e)
        {
            if (shoucanggedan != null && woshoucangStack != null)
            {
                woshoucangStack.Visibility = Visibility.Collapsed;
                shoucanggedan.ToolTip = "展开";
            }
        }

        private void shoucanggedan_Checked(object sender, RoutedEventArgs e)
        {
            if (shoucanggedan != null && woshoucangStack != null)
            {
                woshoucangStack.Visibility = Visibility.Visible;
                shoucanggedan.ToolTip = "收起";
            }
        }

        private void yinxiao_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string checkV = rb.Tag as string;
                string curVisilityButtonName = stb.Name;
                if (checkV != curVisilityButtonName)
                {
                    stb.Visibility = Visibility.Collapsed;
                    yinzhi = false;
                    stb.IsChecked = false;
                    stb = this.FindName(checkV) as AyImageToggleButton;
                    if (stb != null)
                    {
                        stb.Visibility = Visibility.Visible;
                        yinzhi = false;
                        stb.IsChecked = true;
                    }
                }
            }
        }

        private void yinxiao2_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string checkV = rb.Tag as string;
                string curVisilityButtonName = stbYinXiao.Name;
                if (checkV != curVisilityButtonName)
                {
                    stbYinXiao.Visibility = Visibility.Collapsed;
                    yinxiao = false;
                    stbYinXiao.IsChecked = false;
                    stbYinXiao = this.FindName(checkV) as AyImageToggleButton;
                    if (stbYinXiao != null)
                    {
                        stbYinXiao.Visibility = Visibility.Visible;
                        yinxiao = false;
                        stbYinXiao.IsChecked = true;
                    }
                }
            }
        }

        private void nav201_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                yinzhiPopup.IsOpen = false;
                openYinZhiPop = false;

            }
        }


        private void nav301_MouseEnter(object sender, MouseEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                string checkV = rb.Tag as string;
                string uri = null;
                switch (checkV)
                {
                    case "swichyinxiao":
                        uri = "pack://application:,,,/AYQQMusic;component/Contents/Images/rightmenu/xunhuanyin0.png";
                        huanraoSlider.Visibility = Visibility.Collapsed;
                        diyinSlider.Visibility = Visibility.Collapsed;
                        renshengSlider.Visibility = Visibility.Collapsed;
                        xinchangSlider.Visibility = Visibility.Collapsed;
                        chakanxiangqing.Visibility = Visibility.Visible;
                        ruo.Visibility = Visibility.Collapsed;
                        qiang.Visibility = Visibility.Collapsed;
                        break;
                    case "huanrao":
                        uri = "pack://application:,,,/AYQQMusic;component/Contents/Images/rightmenu/xunhuanyin1.png";
                        huanraoSlider.Visibility = Visibility.Visible;
                        diyinSlider.Visibility = Visibility.Collapsed;
                        renshengSlider.Visibility = Visibility.Collapsed;
                        xinchangSlider.Visibility = Visibility.Collapsed;
                        chakanxiangqing.Visibility = Visibility.Collapsed;
                        ruo.Visibility = Visibility.Visible;
                        qiang.Visibility = Visibility.Visible;
                        break;
                    case "diyin":
                        uri = "pack://application:,,,/AYQQMusic;component/Contents/Images/rightmenu/xunhuanyin2.png";
                        huanraoSlider.Visibility = Visibility.Collapsed;
                        diyinSlider.Visibility = Visibility.Visible;
                        renshengSlider.Visibility = Visibility.Collapsed;
                        xinchangSlider.Visibility = Visibility.Collapsed;
                        chakanxiangqing.Visibility = Visibility.Collapsed;
                        ruo.Visibility = Visibility.Visible;
                        qiang.Visibility = Visibility.Visible;
                        break;
                    case "rensheng":
                        uri = "pack://application:,,,/AYQQMusic;component/Contents/Images/rightmenu/xunhuanyin3.png";
                        huanraoSlider.Visibility = Visibility.Collapsed;
                        diyinSlider.Visibility = Visibility.Collapsed;
                        renshengSlider.Visibility = Visibility.Visible;
                        xinchangSlider.Visibility = Visibility.Collapsed;
                        chakanxiangqing.Visibility = Visibility.Collapsed;
                        ruo.Visibility = Visibility.Visible;
                        qiang.Visibility = Visibility.Visible;
                        break;
                    case "xianchang":
                        uri = "pack://application:,,,/AYQQMusic;component/Contents/Images/rightmenu/xunhuanyin4.png";
                        huanraoSlider.Visibility = Visibility.Collapsed;
                        diyinSlider.Visibility = Visibility.Collapsed;
                        renshengSlider.Visibility = Visibility.Collapsed;
                        xinchangSlider.Visibility = Visibility.Visible;
                        chakanxiangqing.Visibility = Visibility.Collapsed;
                        ruo.Visibility = Visibility.Visible;
                        qiang.Visibility = Visibility.Visible;
                        break;
                }
                if (uri == null)
                {
                    yinXiaoContentImg.Source = null;
                    yinxiaoSliderlist.Visibility = Visibility.Collapsed;
                    chakanxiangqing.Visibility = Visibility.Collapsed;
                    ruo.Visibility = Visibility.Collapsed;
                    qiang.Visibility = Visibility.Collapsed;
                    return;
                }
                yinxiaoSliderlist.Visibility = Visibility.Visible;

                yinXiaoContentImg.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));

            }
        }


        private void clickChakan_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NewWebUrl("http://music.qq.com/spaframe/static/voice_introduction.html");
            //web.WebBrowser.Navigate(new Uri("http://music.qq.com/spaframe/static/voice_introduction.html"));
            CloseYinXiao();
        }


        List<string> supportExt = new List<string> {
            ".MP3",".WMA",".WAV",".AAC",".FLAC",".APE",".MID",".OGG"
        };


        public static VlcPlayer CurrentPlayer = new VlcPlayer();
        //int maxDirectoryDetect = 10;//最多3层文件夹
        //int maxDirectoryDetectIndex = 0;

        public void GetAll(DirectoryInfo dir)//搜索文件夹中的文件
        {
            try
            {
                FileInfo[] allFile = dir.GetFiles();
                foreach (FileInfo fi in allFile)
                {
                    string ext = System.IO.Path.GetExtension(fi.Name).ToUpper();
                    if (supportExt.Contains(ext))
                    {
                        string getime = "";
                        if (System.IO.Path.GetExtension(fi.Name).ToLower() == ".mp3")
                        {
                            getime = GetMusicLengthString(fi.FullName);
                            var dd = getime.Split(':');
                            if (dd.Count() == 3)
                            {
                                DateTime dall = new DateTime(1991, 4, 4, 0, 0, 0);
                                dall = dall.AddHours(dd[0].ToInt2());
                                dall = dall.AddMinutes(dd[1].ToInt2());
                                dall = dall.AddSeconds(dd[2].ToInt2());

                                AyThread.Instance.RunUI(() =>
                                {
                                    LoadPlayListBoxData(fi, dall);
                                });


                            }
                            else if (dd.Count() == 2)
                            {
                                DateTime dall = new DateTime(1991, 4, 4, 0, 0, 0);
                                dall = dall.AddMinutes(dd[0].ToInt2());
                                dall = dall.AddSeconds(dd[1].ToInt2());

                                LoadPlayListBoxData(fi, dall);
                            }
                        }
                        else
                        {

                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                VlcPlayer Player = new VlcPlayer();
                                Player.Initialize(@"Contents\LibVlc", new string[] { "-I", "--dummy", "--ignore-config", "--no-video-title", "--no-audio" });
                                Player.LoadMedia(fi.FullName);
                                AyTime.setTimeout(800, () =>
                                {
                                    var totalDuration = Player.GetDuration();
                                    DateTime dall = new DateTime(1991, 4, 4, 0, 0, 0);
                                    dall = dall.AddHours(totalDuration.Hours);
                                    dall = dall.AddMinutes(totalDuration.Minutes);
                                    dall = dall.AddSeconds(totalDuration.Seconds);
                                    LoadPlayListBoxData(fi, dall);
                                    Player.Dispose();

                                });
                            }
                            ));



                        }


                    }

                }

                //DirectoryInfo[] allDir = dir.GetDirectories();
                //foreach (DirectoryInfo d in allDir)
                //{
                //    maxDirectoryDetectIndex++;
                //    if (maxDirectoryDetectIndex > maxDirectoryDetect)
                //    {
                //        maxDirectoryDetectIndex = 0;
                //        break;
                //    }
                //    GetAll(d);

                //}
            }
            catch
            {

            }

        }
        public static AyImageToggleButton localAlbum;
        static int fileCnt = 0;
        static string temp_album = null;
        public static void SetAlbumImage(ImageSource albumSource, string localImagePath = null)
        {
            if (albumSource != null)
            {
                localAlbum.Icon = albumSource;
                //请求web，下载专辑图片

            }
            else
            {
                localAlbum.Icon = DefaultAlbumImage;
                geAlbum.Source = "pack://application:,,,/AYQQMusic;component/Contents/Images/Album/defaultAlbum.png";
            }
            if (localImagePath.IsNotNull())
            {
                if (geAlbum != null)
                {
                    int ad = 0;
                    DispatcherTimer ds = new DispatcherTimer();
                    ds = AyTime.setInterval(500, () =>
                       {
                           ad++;
                           if (ad > 10)
                           {
                               ds.Stop();
                               ds = null;
                           }
                           if (System.IO.File.Exists(localImagePath))
                           {
                               geAlbum.Source = localImagePath;
                               ds.Stop();
                               ds = null;
                           }
                       });

                }
                else
                {
                    temp_album = localImagePath;
                }
            }


        }
        private static void LoadPlayListBoxData(FileInfo fi, DateTime dall)
        {
            AyThread.Instance.RunUI(() =>
            {
                PlayListItemModel model = new PlayListItemModel();
                model.ExtName = System.IO.Path.GetExtension(fi.FullName);
                model.SongGuid = AyExtension.GetGuid;
                model.SongName = fi.Name;
                model.SongNameWithoutExt = System.IO.Path.GetFileNameWithoutExtension(model.SongName);
                model.SongPath = fi.FullName;
                model.AllPlayTime = dall;
                MainWindow.curr.PlayQueue.Items.Add(model);
                fileCnt++;
            });

        }

        #region 后台处理文件

        private void getFileList_bw(DirectoryInfo dir)
        {
            BackgroundWorker m_bgWorker = new BackgroundWorker();
            m_bgWorker.DoWork += new DoWorkEventHandler(m_bgWorker_DoWork);
            m_bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler
                        (m_bgWorker_RunWorkerCompleted);
            m_bgWorker.RunWorkerAsync(dir);
        }


        private void m_bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DirectoryInfo dirname = (DirectoryInfo)e.Argument;
            GetAll(dirname);
        }
        static int lastFileCnt = 0;
        private void m_bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            if (e.Cancelled)
            {

            }
            else
            {
                //监视数值 ay 2016-6-2 23:22:30
                var ct = AyTime.setInterval(1000, () =>
                  {
                      lastFileCnt = fileCnt;
                  });
                DispatcherTimer ct1 = new DispatcherTimer();
                ct1 = AyTime.setInterval(2000, () =>
               {
                   if (lastFileCnt == fileCnt)
                   {
                       ct.Stop();
                       ct = null;
                       ct1.Stop();
                       ct1 = null;
                       lastFileCnt = fileCnt = 0;

                       SaveLocalPlayData();

                   }
               });
            }
        }

        #endregion

        private void AyTextButton_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                //遍历文件，获取歌曲文件
                curr.PlayQueue.Items.Clear();
                DirectoryInfo d = new DirectoryInfo(foldPath);
                getFileList_bw(d);
            }

            e.Handled = true;
        }


        public static void SaveLocalPlayData()
        {
            List<PlayListItemModelSerial> ps = new List<PlayListItemModelSerial>();
            foreach (var item in curr.PlayQueue.Items)
            {
                PlayListItemModelSerial pim = new PlayListItemModelSerial();
                pim.SongGuid = item.SongGuid;
                pim.SongId = item.SongId;
                pim.SongName = item.SongName;
                pim.SongNameWithoutExt = item.SongNameWithoutExt;
                pim.SongPath = item.SongPath;
                pim.SongSinger = item.SongSinger;
                pim.SongSingerId = item.SongSingerId;
                pim.ExtName = item.ExtName;
                pim.AllPlayTime = item.AllPlayTime;
                pim.LrcFileName = item.LrcFileName;
                pim.LrcNeedDown = item.LrcNeedDown;
                pim.LrcPath = item.LrcPath;
                pim.LoveStatus = item.LoveStatus;
                pim.PlayStatus = false;
                pim.CurrentPlayTime = item.CurrentPlayTime;
                pim.BiaoZhunYinZhi = item.BiaoZhunYinZhi;
                pim.HqYinZhi = item.HqYinZhi;
                pim.SqAPE = item.SqAPE;
                pim.SqFLAC = item.SqFLAC;
                pim.FLAC51 = item.FLAC51;
                ps.Add(pim);
            }
            Mp3SerializeHelper.SaveCurrentPlay(ps, ExtendUtils.GetDATA_PATH() + ExtendUtils.CurrentPlayDbName);
        }
        public PlayListItemModel defaultNullPlay = new PlayListItemModel();
        private void AyTextButton_Click_1(object sender, RoutedEventArgs e)
        {
            curr.SetPlayLock();
            CurrentPlayer.Stop();
            CurrentPlayer.Dispose();
            MainWindow.curr.CurrentTime = "00:00";
            MainWindow.curr.TotalTime = "00:00";
            MainWindow.curr.GeName = "AYUI音乐，听我想听的歌";
            CurSongGuid = "";
            MusicMainWindowModel.lastMusic = null;
            MainWindow.curr.Singer = "";
            SetAlbumImage(DefaultAlbumImage);
            musicProgress.Value = 0;
            curr.ProgressEnbaled = false;
            curr.WorldPlayStatus = false;
            MusicMainWindowModel.currentPlayTimer.Stop();
            curr.PlayQueue.Items.Clear();
            SaveLocalPlayData();
        }
        bool cl = true;



        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (cl)
            {
                CurrentPlayer.Dispose();
                ApiManager.ReleaseAll();
                cl = false;
            }

        }


        #region 播放列表管理
        public static BitmapImage DefaultAlbumImage;
        private void GengDuo_Click(object sender, RoutedEventArgs e)
        {
            AySplitButton sb = sender as AySplitButton;
            if (sb != null)
            {
                sb.ContextMenu.IsOpen = true;
                var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(sb);
                if (bl != null)
                {
                    IList selectedItems = lb_bfdl.GetSelectedItems() as IList;
                    System.Reflection.PropertyInfo propertyInfo;
                    int curentEmeCnt = selectedItems.Count;
                    if (curentEmeCnt == 0)
                    {
                        bl.IsSelected = true;
                        return;
                    }
                    else
                    {
                        Type type = selectedItems[0].GetType();
                        propertyInfo = type.GetProperty("Selected");
                        for (int i = 0; i < curentEmeCnt; i++)
                        {
                            var item = selectedItems[i];
                            propertyInfo.SetValue(item, false, null);
                        }
                        bl.IsSelected = true;
                    }


                }
            }
            e.Handled = true;
        }


        //TODO 播放GET
        private void playlist_play_Checked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDPLAYLOCK)
            {
                return;
            }
            AyImageToggleButton sb = sender as AyImageToggleButton;
            if (sb.IsNotNull())
            {
                var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(sb);
                if (bl.IsNotNull())
                {
                    IList selectedItems = lb_bfdl.GetSelectedItems() as IList;
                    System.Reflection.PropertyInfo propertyInfo;
                    int curentEmeCnt = selectedItems.Count;
                    if (curentEmeCnt == 0)
                    {
                        bl.IsSelected = true;
                    }
                    else
                    {
                        Type type = selectedItems[0].GetType();
                        propertyInfo = type.GetProperty("Selected");
                        for (int i = 0; i < curentEmeCnt; i++)
                        {
                            var item = selectedItems[i];
                            propertyInfo.SetValue(item, false, null);
                        }
                        bl.IsSelected = true;
                    }
                    PlayListItemModel plm = bl.DataContext as PlayListItemModel;
                    if (plm.IsNotNull())
                    {
                        if (plm.SongGuid == CurSongGuid)
                        {
                            curr.SetPlayLock();
                            StartSecondPlayer();
                            curr.WorldPlayStatus = true;
                            curr.ProgressEnbaled = true;
                            CurrentPlayer.PauseOrResume();
                        }
                        else
                        {
                            if (System.IO.File.Exists(plm.SongPath))
                            {
                                curr.SetPlayLock();
                                curr.WorldPlayStatus = true;
                                curr.ProgressEnbaled = true;
                            }

                            MusicMainWindowModel.PlayAyMusic(plm);
                        }

                    }


                }
            }
            e.Handled = true;

        }

        /// <summary>
        /// 开始播放时间计时器
        /// </summary>
        private static void StartSecondPlayer()
        {
            MusicMainWindowModel.currentPlayTimer.Start();
        }

        private void playlist_play_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDPLAYLOCK)
            {
                return;
            }
            playlist_uncheckplay();
        }

        private static void playlist_uncheckplay()
        {
            curr.SetPlayLock();
            curr.WorldPlayStatus = false;
            CurrentPlayer.PauseOrResume();
            if (MusicMainWindowModel.lastMusic.IsNotNull())
            {
                curr.SetPlayLock();
                MusicMainWindowModel.lastMusic.PlayStatus = false;
            }

        }


        //TODO 播放GET
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDPLAYLOCK)
            {
                return;
            }
            if (CurSongGuid == "")
            {
                if (curr.PlayQueue.Items.Count > 0)//有列表，播放第一首歌曲
                {
                    var plm = curr.PlayQueue.Items[0];
                    if (plm.IsNotNull())
                    {
                        plm.Selected = true;

                        MusicMainWindowModel.PlayAyMusic(plm);
                        curr.ProgressEnbaled = true;

                    }
                }
                else
                {
                    curr.SetPlayLock();
                    rbPlay.IsChecked = false;
                    AyMessageBox.ShowInformation("您的播放列表一首歌曲也没有,请先导入歌曲.");

                }
            }
            else
            {
                if (MusicMainWindowModel.lastMusic.IsNotNull())
                {
                    curr.SetPlayLock();
                    StartSecondPlayer();
                    MusicMainWindowModel.lastMusic.PlayStatus = true;
                    curr.ProgressEnbaled = true;
                    CurrentPlayer.PauseOrResume();
                }

            }


        }

        //private void SetAlbum(string source)
        //{
        //    albumControl.Source = "";
        //}

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDPLAYLOCK)
            {
                return;
            }

            playlist_uncheckplay();

        }
        private void woxihuan_Checked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDLOVELOCK)
            {
                return;
            }
            if (MusicMainWindowModel.lastMusic.IsNotNull())
            {
                curr.SetLoveLock();
                MusicMainWindowModel.lastMusic.LoveStatus = true;
                SaveLocalPlayData();
            }
        }
        private void woxihuan_Unchecked(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDLOVELOCK)
            {
                return;
            }

            if (MusicMainWindowModel.lastMusic.IsNotNull())
            {
                curr.SetLoveLock();
                MusicMainWindowModel.lastMusic.LoveStatus = false;
                SaveLocalPlayData();
            }
        }



        private void playlist_like_Checked(object sender, RoutedEventArgs e)
        {
            MusicLoveMethod(sender, e);
        }

        private static void MusicLoveMethod(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDLOVELOCK)
            {
                return;
            }
            curr.SetLoveLock();
            AyImageToggleButton sb = sender as AyImageToggleButton;
            if (sb.IsNotNull())
            {
                var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(sb);
                PlayListItemModel plm = bl.DataContext as PlayListItemModel;
                if (plm.IsNotNull())
                {
                    plm.LoveStatus = true;
                    SaveLocalPlayData();
                    if (CurSongGuid == plm.SongGuid)
                    {
                        curr.SetLoveLock();
                        curr.WorldLoveStatus = true;
                    }
                }
            }
            e.Handled = true;
        }

        private void playlist_like_Unchecked(object sender, RoutedEventArgs e)
        {
            MusicUnLoveMethod(sender, e);
        }

        private static void MusicUnLoveMethod(object sender, RoutedEventArgs e)
        {
            if (MusicMainWindowModel.WORLDLOVELOCK)
            {
                return;
            }
            curr.SetLoveLock();
            AyImageToggleButton sb = sender as AyImageToggleButton;
            if (sb.IsNotNull())
            {
                var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(sb);
                PlayListItemModel plm = bl.DataContext as PlayListItemModel;
                if (plm.IsNotNull())
                {
                    plm.LoveStatus = false;
                    SaveLocalPlayData();
                    if (CurSongGuid == plm.SongGuid)
                    {
                        curr.SetLoveLock();
                        curr.WorldLoveStatus = false;
                    }
                }
            }
            e.Handled = true;
        }
        #endregion

        private void musicProgress_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //var value = (float)(e.GetPosition(musicProgress).X / musicProgress.ActualWidth);
            CurrentPlayer.Position = musicProgress.Value.ToFloat();
            curr.sliderProgressLock = false;
        }

        private void musicProgress_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            curr.sliderProgressLock = true;
        }

        private void nextMusic_Click(object sender, RoutedEventArgs e)
        {
            NextMusic();
        }
        private void lastMusic_Click(object sender, RoutedEventArgs e)
        {
            LastMusic();
        }

        WpfTreeHelper wh = new WpfTreeHelper();
        public void LastMusic()
        {
            if (lb_bfdl.Items.Count == 0)
            {
                return;
            }
            if (lb_bfdl.SelectedIndex > 0)
            {
                lb_bfdl.SelectedIndex = lb_bfdl.SelectedIndex - 1;
                var music = lb_bfdl.SelectedItem as PlayListItemModel;
                ListBoxItem lstitem = lb_bfdl.ItemContainerGenerator.ContainerFromItem(music) as ListBoxItem;
                if (lstitem != null)
                {
                    var dfsd = wh.FindFirstVisualChildFromDataTemplate<AyImageToggleButton>(lstitem, "playlist_play");
                    if (dfsd != null)
                    {
                        var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(dfsd);
                        bl.BringIntoView();
                    }
                }

                curr.SetPlayLock();
                MusicMainWindowModel.PlayAyMusic(music);
            }
            else
            {
                AyMessageBox.ShowInformation("已经是第一首歌曲了.");
            }
        }
        public void NextMusic()
        {
            if (lb_bfdl.Items.Count == 0)
            {
                return;
            }
            if (lb_bfdl.SelectedIndex < lb_bfdl.Items.Count - 1)
            {
                lb_bfdl.SelectedIndex = lb_bfdl.SelectedIndex + 1;
                var music = lb_bfdl.SelectedItem as PlayListItemModel;
                ListBoxItem lstitem = lb_bfdl.ItemContainerGenerator.ContainerFromItem(music) as ListBoxItem;
                if (lstitem != null)
                {
                    var dfsd = wh.FindFirstVisualChildFromDataTemplate<AyImageToggleButton>(lstitem, "playlist_play");
                    if (dfsd != null)
                    {
                        var bl = WpfTreeHelper.FindParentControl<ListBoxItem>(dfsd);
                        bl.BringIntoView();
                    }
                }

                curr.SetPlayLock();
                MusicMainWindowModel.PlayAyMusic(music);
            }
            else
            {
                AyMessageBox.ShowInformation("已经是最后一首歌曲了.");
            }

        }

        static UcAlbumLrc geAlbum;
        IEasingFunction easyOut = new CircleEase() { EasingMode = EasingMode.EaseOut };
        IEasingFunction easyIn = new CircleEase() { EasingMode = EasingMode.EaseIn };
        private void albumButton_Checked(object sender, RoutedEventArgs e)
        {
            double canvasFromheight = 0;
            canvasFromheight = InitgeAlbum(canvasFromheight);
            if (string.IsNullOrEmpty(CurSongGuid))
            {
                geAlbum.Source = "pack://application:,,,/AYQQMusic;component/Contents/Images/Album/defaultAlbum.png";
            }
            else
            {
                if (string.IsNullOrEmpty(geAlbum.Source) && !string.IsNullOrEmpty(temp_album))
                {
                    geAlbum.Source = temp_album;
                }

            }
            geAlbum.Visibility = Visibility.Visible;
            geAlbum.BeginAnimation(Canvas.BottomProperty, HeightKey);

        }

        private double InitgeAlbum(double canvasFromheight)
        {
            if (geAlbum == null)
            {
                geAlbum = new UcAlbumLrc();
                geAlbum.GaoSi = 50;
                Binding binding = new Binding { Path = new PropertyPath("ActualWidth"), Source = AblumLrc };
                geAlbum.SetBinding(UcAlbumLrc.WidthProperty, binding);
                Binding binding2 = new Binding { Path = new PropertyPath("ActualHeight"), Source = AblumLrc };
                geAlbum.SetBinding(UcAlbumLrc.HeightProperty, binding2);

                AyExtension.SetAyWindowMouseLeftButtonCommonClick(this, geAlbum, () =>
                {
                    if (this.WindowState == WindowState.Normal)
                    {
                        geAlbum.maxWindow.IsChecked = true;
                    }
                    else
                    {
                        geAlbum.maxWindow.IsChecked = false;
                    }
                });
                canvasFromheight = AblumLrc.ActualHeight * (-1);
                Canvas.SetBottom(geAlbum, canvasFromheight);
                AblumLrc.Children.Add(geAlbum);
            }

            return canvasFromheight;
        }

        private void HeightKey_Completed(object sender, EventArgs e)
        {
            Canvas.SetBottom(geAlbum, 0);
            base.WindowRightButtonGroupVisibility = Visibility.Collapsed;
            Animation_ControlBarUp();
        }

        private void albumButton_Unchecked(object sender, RoutedEventArgs e)
        {
            double canvasFromheight = 0;
            double mintime = 200;
            DoubleAnimationUsingKeyFrames CloseHeightKey = new DoubleAnimationUsingKeyFrames();
            CloseHeightKey.Duration = new Duration(TimeSpan.FromMilliseconds(mintime));
            CloseHeightKey.FillBehavior = FillBehavior.Stop;
            canvasFromheight = AblumLrc.ActualHeight * (-1);
            EasingDoubleKeyFrame HeightKey0 = new EasingDoubleKeyFrame(canvasFromheight, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(mintime)));
            HeightKey0.EasingFunction = easyIn;
            CloseHeightKey.KeyFrames.Add(HeightKey0);
            CloseHeightKey.Completed += (a, ed) =>
            {
                Canvas.SetBottom(geAlbum, canvasFromheight);
                geAlbum.Visibility = Visibility.Collapsed;
                base.WindowRightButtonGroupVisibility = Visibility.Visible;
                Animation_ControlBarDown();
            };
            geAlbum.BeginAnimation(Canvas.BottomProperty, CloseHeightKey);
        }

    }



}
