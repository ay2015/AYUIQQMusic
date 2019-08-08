using CefSharp;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net;
using CefSharp.Wpf;

namespace Ay.Framework.WPF.Controls
{
    /// <summary>
    /// Interaction logic for WebPageViewer.xaml
    /// </summary>
    public partial class WebPageViewer : UserControl, IRequestHandler
    {
        private WebView _view;

        public WebPageViewer(string url)
        {
            InitializeComponent();

            CEF.Initialize(new Settings { LogSeverity = LogSeverity.Disable, PackLoadingDisabled = true });

            BrowserSettings browserSetting = new BrowserSettings { FullscreenEnabled=true,ApplicationCacheDisabled = true, PageCacheDisabled = true };

            _view = new WebView(string.Empty, browserSetting)
            {
                Address = url,
                RequestHandler = this,
                Background = Brushes.White
            };
            _view.RegisterJsObject("callbackObj", new CallbackObjectForJs());

            //_view.LoadCompleted += _view_LoadCompleted;

            MainGrid.Children.Insert(0, _view);
        }

        private void _view_LoadCompleted(object sender, LoadCompletedEventArgs url)
        {
            //Dispatcher.BeginInvoke(new Action(() => 
            //{
            //    maskLoading.Visibility = Visibility.Collapsed;
            //}));
        }

        public void View(string url)
        {
            if (_view.IsBrowserInitialized)
            {
                _view.Visibility = Visibility.Hidden;

                //maskLoading.Visibility = Visibility.Visible;

                _view.Load(url);
            }
        }
        public void GoForward()
        {
            _view.Forward();
        }
        public void GoBack()
        {
            _view.Back();
        }
        public void Refresh(bool ignoreCache=false)
        {
            _view.Reload(ignoreCache);
        }

        #region IRequestHandler
        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
        {
            return true;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers)
        {

        }
        #endregion
    }

    public class CallbackObjectForJs
    {
        public void showMessage(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
