using HongKongSchools.DataParser.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HongKongSchools.DataParser.Services.XMLReaderService
{
    public interface IXMLReaderService
    {
        IEnumerable<T> Read<T>(string filePath) where T : XMLFile, new();
    }
}
