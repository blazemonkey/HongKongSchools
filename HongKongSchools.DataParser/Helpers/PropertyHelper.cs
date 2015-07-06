using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Helpers
{
    public class PropertyHelper
    {
        public static string SetStringProperty(string value)
        {
            return !string.IsNullOrEmpty(value) ? value.Trim().Normalize(NormalizationForm.FormKC) : "";
        }

        public static DateTime? SetDateTimeProperty(string value)
        {
            return !string.IsNullOrEmpty(value) ? DateTime.Parse(value) : default(DateTime);
        }        
    }
}
