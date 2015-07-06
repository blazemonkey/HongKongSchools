using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Abstracts
{
    public abstract class XMLFile
    {
        public abstract string RootElement { get; }
        public abstract string ObjectElement { get; }
        public abstract void SetProperty(string column, string value);
    }
}
