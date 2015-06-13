using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace HongKongSchools.Converters
{
    public class LatitudeLongitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(string))
                return string.Empty;

            var coordinate = value.ToString();
            var DMS = coordinate.Split('-');

            return string.Format("{0}{1}{2}{3}{4}{5}{6}", DMS[0], "°", DMS[1], "'", DMS[2], "\"", parameter.ToString());
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
