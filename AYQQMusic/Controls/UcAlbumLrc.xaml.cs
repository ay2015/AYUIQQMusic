using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ay.Framework.WPF.Controls;

namespace AYQQMusic.Controls
{
    /// <summary>
    /// UcAlbumLrc.xaml 的交互逻辑
    /// </summary>
    public partial class UcAlbumLrc : UserControl
    {
        public double GaoSi
        {
            get { return (double)GetValue(GaoSiProperty); }
            set { SetValue(GaoSiProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GaoSi.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GaoSiProperty =
            DependencyProperty.Register("GaoSi", typeof(double), typeof(UcAlbumLrc), new PropertyMetadata(0.00));


        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(UcAlbumLrc), new PropertyMetadata(null));

        public UcAlbumLrc()
        {
            InitializeComponent();
            Loaded += UcAlbumLrc_Loaded;
        }
        MainWindow win;
        void UcAlbumLrc_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= UcAlbumLrc_Loaded;
            win = Window.GetWindow(this) as MainWindow;
        }

        private void minwindow_Click(object sender, RoutedEventArgs e)
        {
            win.DoMinWindow();  
        }

        private void downAlbum_Click(object sender, RoutedEventArgs e)
        {
            win.albumButton.IsChecked = false;
        }

        private void maxWindow_Checked(object sender, RoutedEventArgs e)
        {
            win.IsCoverTaskBar = true;
            win.MaxWindowMethodOverride(); 
        }

        private void maxWindow_Unchecked(object sender, RoutedEventArgs e)
        {
            win.IsCoverTaskBar = false;
            win.MaxWindowMethodOverride();
        }
    }
}
