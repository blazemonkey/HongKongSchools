using HongKongSchools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace HongKongSchools.Converters
{
    public class SessionsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() != typeof(List<Session>))
                return "";

            var sessions = value as List<Session>;
            var text = "";

            foreach (var s in sessions)
            {
                text = text + s.Name + ",";
            }

            return text.Substring(0, text.Length -1);
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
