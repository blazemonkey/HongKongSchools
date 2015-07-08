using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class ManualUpdate
    {
        public int Type { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
