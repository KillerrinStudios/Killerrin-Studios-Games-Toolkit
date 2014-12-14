using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace KillerrinStudiosToolkit.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public static Visibility ConvertToVisibility(bool value)
        {
            if ((bool)value)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public static bool ConvertToBoolean(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Collapsed:
                    return false;
                case Visibility.Visible:
                    return true;
            }
            throw new Exception();
        }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return ConvertToVisibility((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return ConvertToBoolean((Visibility)value);
        }
    }
}
