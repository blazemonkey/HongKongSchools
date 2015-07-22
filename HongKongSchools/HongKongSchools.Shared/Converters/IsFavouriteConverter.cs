using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace HongKongSchools.Converters
{
    public class IsFavouriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(bool))
                return App.Current.Resources["AppBrandColor"];

            var boolean = System.Convert.ToBoolean(value.ToString());

            return boolean == true ? App.Current.Resources["AppBrandColor"] : new SolidColorBrush(Colors.Black);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
