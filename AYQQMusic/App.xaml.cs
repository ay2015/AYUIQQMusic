using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace AYQQMusic
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.AddResourceDictionary("/AYQQMusic;component/Contents/Styles/QQMusic.xaml").AYUI();
            base.OnStartup(e);
        }
    }
}
