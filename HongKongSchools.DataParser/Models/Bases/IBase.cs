using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Bases
{
    public interface IBase
    {
        int Id { get; set; }
        int GroupId { get; set; }
        string Name { get; set; }
        int LanguageId { get; set; }
    }
}
