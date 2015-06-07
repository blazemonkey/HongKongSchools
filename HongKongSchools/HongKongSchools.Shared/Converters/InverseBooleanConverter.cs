using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace HongKongSchools.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((value == null) || (value.GetType() != typeof(bool)))
                return null;

            return !System.Convert.ToBoolean(value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
