using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Xaml.Data;

namespace KillerrinStudiosToolkit.Converters
{
    public class RelativeDateTimeConverter : IValueConverter
    {
        private const int Minute = 60;
        private const int Hour = Minute * 60;
        private const int Day = Hour * 24;
        private const int Year = Day * 365;

        /// <summary>
        /// Converts the DateTime to a relative value sentence
        /// </summary>
        /// <param name="utcValue"The Time in UTC></param>
        /// <returns>A string with the time relative value in a string</returns>
        public static string CalculateConversion(DateTime utcValue)
        {
            var difference = DateTime.UtcNow - utcValue;

            string result = "";
            if (difference.TotalSeconds < 2.0)
                result = "a second ago";
            else if (difference.TotalSeconds < Minute)
                result = Math.Floor(difference.TotalSeconds) + " seconds ago";

            else if (difference.TotalSeconds < Minute * 2)
                result = "a minute ago";
            else if (difference.TotalSeconds < Hour)
                result = Math.Floor(difference.TotalMinutes) + " minutes ago";

            else if (difference.TotalSeconds < Hour * 2)
                result = "an hour ago";
            else if (difference.TotalSeconds < Day)
                result = Math.Floor(difference.TotalHours) + " hours ago";

            else if (difference.TotalSeconds < Day * 2)
                result = "yesterday";
            else if (difference.TotalSeconds < Day * 30)
                result = Math.Floor(difference.TotalDays) + " days ago";

            else if (difference.TotalSeconds < Day * 60)
                result = "a month ago";
            else if (difference.TotalSeconds < Year)
                result = Math.Floor(difference.TotalDays / 30) + " months ago";

                // And because no one cares once its past a certain point, just display a year
            else result = utcValue.ToString();

            return result;
        }

        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            var dateTime = (DateTime)value;
            string result = CalculateConversion(dateTime);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotSupportedException();
        }
    }
}
