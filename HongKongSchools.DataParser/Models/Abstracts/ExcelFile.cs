using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Abstracts
{
    public abstract class ExcelFile
    {
        public abstract int ColumCount { get; }
        public abstract void SetProperty(int column, string value);
    }
}
