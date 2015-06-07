using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class Religion : IBase
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }
    }
}
