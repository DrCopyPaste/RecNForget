using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace RecNForget.Controls
{
    [ValueConversion(typeof(TimeSpan), typeof(String))]
    public class StringToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            // TODO something like:
            return ((TimeSpan)value).ToString("d':'hh':'mm':'ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  CultureInfo culture)
        {
            return TimeSpan.ParseExact(
                input: (string)value,
                format: "d':'hh':'mm':'ss",
                formatProvider: culture);
        }
    }
}
