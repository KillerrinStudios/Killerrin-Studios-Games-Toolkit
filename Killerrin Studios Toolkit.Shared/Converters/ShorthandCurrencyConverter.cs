using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace KillerrinStudiosToolkit.Converters
{
    public class ShorthandCurrencyConverter : IValueConverter
    {
        public static string ConvertToShorthand(double value)
        {
            string stringValue = value.ToString();

            if (value > 100000000000) return stringValue.Substring(0, 3) + "B";
            if (value > 10000000000) return stringValue.Substring(0, 2) + "B";
            if (value > 1000000000) return stringValue.Substring(0, 1) + "B";
            if (value > 100000000) return stringValue.Substring(0, 3) + "M";
            if (value > 10000000) return stringValue.Substring(0, 2) + "M";
            if (value > 1000000) return stringValue.Substring(0, 1) + "M";
            if (value > 100000) return stringValue.Substring(0, 3) + "K";
            if (value > 10000) return stringValue.Substring(0, 2) + "K";
            if (value > 1000) return stringValue.Substring(0, 1) + "K";
            return stringValue;
        }
        public static string ConvertBackToRegularForm(string value)
        {
            return "";
        }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return ConvertToShorthand((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return ConvertBackToRegularForm((string)value);
        }
    }
}
