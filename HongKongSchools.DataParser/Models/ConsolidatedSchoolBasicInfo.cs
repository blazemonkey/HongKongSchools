using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class ConsolidatedSchoolBasicInfo
    {
        public uint SchoolNumber { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SchoolLevel { get; set; }
        public List<string> SchoolSessions { get; set; }

        public ConsolidatedSchoolBasicInfo()
        {
            SchoolSessions = new List<string>();
        }
    }
}
