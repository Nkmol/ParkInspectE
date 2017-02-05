using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ParkInspect.Convertor
{
    internal class BitmapConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null
                ? null
                : BitmapFrame.Create(
                    new Uri((string) value),
                    BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}