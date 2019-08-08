using Ay.Framework.WPF.Controls;
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
using System.Windows.Shapes;

namespace AYQQMusic
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
            Loaded += Window3_Loaded;
            demoColor.MouseDown += new MouseButtonEventHandler(darkColor_MouseUp);
            demoBrush.Color = Colors.Transparent;

        }
        void darkColor_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ColorPickerDialog dlg = new ColorPickerDialog();
            dlg.StartingColor = demoBrush.Color;
            dlg.ShowDialog();
            if (dlg.DialogResult == true)
            {
                demoBrush.Color = dlg.SelectedColor;
                gs.BorderBrush = new SolidColorBrush(dlg.SelectedColor);
            }
            e.Handled = true;
        }
        void Window3_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Window3_Loaded;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gs.Source = ttt.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gs.CornerRadius = new CornerRadius(yuanjiao.Text.ToDouble());
        }
    }
}
