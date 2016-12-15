using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ParkInspect.Convertor
{
    class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tempValue = false;

            if (value is bool) tempValue = (bool)value;


            return tempValue ? Visibility.Visible : Visibility.Hidden;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as Visibility? == Visibility.Visible;
        }
    }
}
