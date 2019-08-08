using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ay.Framework.WPF.Controls;
using Meta.Vlc.Wpf;

namespace AYQQMusic
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        public Window2()
        {
            InitializeComponent();
            Loaded += Window2_Loaded;
            //this.SourceInitialized += new EventHandler(AyWindow_SourceInitialized);
        
        }
        VlcPlayer CurrentPlayer = new VlcPlayer();
        #region 这一部分用于最大化时不遮蔽任务栏
        private void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);


            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
        private HwndSource hs;

        private void AyWindow_SourceInitialized(object sender, EventArgs e)
        {
            hs = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            hs.AddHook(new HwndSourceHook(WndProc));
        }
        public const int WM_FALSE = 0;
        public const int WM_TRUE = 1;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:/* WM_GETMINMAXINFO */
                        WmGetMinMaxInfo(hwnd, lParam);
                        handled = true;
                    break;
                case 0x0086:
                    if (wParam == (IntPtr)WM_FALSE)
                    {
                        return (IntPtr)WM_TRUE;
                    }
                    break;
                case 0x0085:
                    return (System.IntPtr)0;

                case 0x0084:
                    #region 注释部分
                    //int x = lParam.ToInt32() & 0xffff;
                    //int y = lParam.ToInt32() >> 16;
                    //double w = this.ActualWidth;
                    //double h = this.ActualHeight;

                    //if (x <= relativeClip & y <= relativeClip) // left top
                    //{
                    //    return (IntPtr)Win32.HTTOPLEFT;
                    //}

                    //else if (x >= w - relativeClip & y <= relativeClip) //right top
                    //{
                    //    return (IntPtr)Win32.HTTOPRIGHT;
                    //}

                    //else if (x >= w - relativeClip & y >= h - relativeClip) //bottom right
                    //{
                    //    return (IntPtr)Win32.HTBOTTOMRIGHT;
                    //}

                    //else if (x <= relativeClip & y >= h - relativeClip)  // bottom left
                    //{
                    //    return (IntPtr)Win32.HTBOTTOMLEFT;
                    //}

                    //else if ((x >= relativeClip & x <= w - relativeClip) & y <= relativeClip) //top
                    //{
                    //    return (IntPtr)Win32.HTTOP;
                    //}

                    //else if (x >= w - relativeClip & (y >= relativeClip & y <= h - relativeClip)) //right
                    //{
                    //    return (IntPtr)Win32.HTRIGHT;
                    //}

                    //else if ((x >= relativeClip & x <= w - relativeClip) & y > h - relativeClip) //bottom
                    //{
                    //    return (IntPtr)Win32.HTBOTTOM;
                    //}

                    //else if (x <= relativeClip & (y <= h - relativeClip & y >= relativeClip)) //left
                    //{
                    //    return (IntPtr)Win32.HTLEFT;
                    //}
                    //else
                    //{
                    //    return (IntPtr)Win32.HTCAPTION;
                    //}
                    #endregion
                    break;
                case 0x0083:
                    return (System.IntPtr)0;

                default: break;
            }
            return (System.IntPtr)0;
        }


        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion
        private void Window2_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Window2_Loaded;
          
            CurrentPlayer.Initialize(@"Contents\LibVlc", new string[] { "-I", "--dummy", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file" });
            CurrentPlayer.EndBehavior = EndBehavior.Repeat;
            WebPageViewer cefWeb = new WebPageViewer("http://i.y.qq.com/v8/fcg-bin/v8_cp.fcg?channel=first&format=html&page=index&tpl=v12");
            this.al.Children.Add(cefWeb);
            Button btn = new Button();
            btn.Width = 100;
            btn.Height = 30;
            btn.Background = new SolidColorBrush(Colors.Red);
            btn.Click += Btn_Click;
            this.al.Children.Add(btn);

            //Button btn1 = new Button();
            //btn1.Margin = new Thickness(100,0,0,0);
            //btn1.Width = 100;
            //btn1.Height = 30;
            //btn1.Background = new SolidColorBrush(Colors.Green);
            //btn1.Click += Btn1_Click; ;
            //this.al.Children.Add(btn1);
        }
        int i = 1;
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
        
            //if (i % 2 == 0)
            //{
            //    this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            //}
            //else {
            //    this.MaxHeight = SystemParameters.PrimaryScreenHeight;
            //}
            //i++;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlayer.Stop();
            CurrentPlayer.LoadMedia(@"F:\Kugou\Anya Marina - Whatever You Like.mp3");
            CurrentPlayer.Play();
            //if (this.WindowState == WindowState.Maximized)
            //{
            //    this.WindowState = WindowState.Normal;
            //}
            //else
            //{
            //    this.WindowState = WindowState.Maximized;
            //}
        }
    }
}
