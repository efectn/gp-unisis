using System;
using System.Globalization;
using System.Windows.Data;

namespace gp_unisis.Converters
{
    public class BoolToEvetHayirConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Evet" : "Hayır";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().ToLower() == "evet";
        }
    }
}