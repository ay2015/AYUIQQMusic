using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace AYQQMusic.Converter
{
    public class PlayToolTipConverter : IValueConverter
    {
        private string trueToolTip;

        public string TrueToolTip
        {
            get { return trueToolTip; }
            set { trueToolTip = value; }
        }
        private string falseToolTip;

        public string FalseToolTip
        {
            get { return falseToolTip; }
            set { falseToolTip = value; }
        }

        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool a = (bool)value;
            if (a)
            {
                return TrueToolTip;
            }
            else
            {
                return FalseToolTip;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = value as string;
            if (a == TrueToolTip)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

}
