using HongKongSchools.DataParser.Models.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HongKongSchools.DataParser.Services.XMLReaderService
{
    public class XMLReaderService : IXMLReaderService
    {
        public IEnumerable<T> Read<T>(string filePath) where T : XMLFile, new()
        {
            var document = XDocument.Load(filePath);
            var inputs = new List<T>();
            var model = new T();
            var rootElement = model.RootElement;
            var objectElement = model.ObjectElement;

            var elements = document.Element(rootElement).Elements(objectElement).ToList();            

            foreach (var e in elements)
            {
                var input = new T();

                foreach (var ee in e.Elements())
                {
                    input.SetProperty(ee.Name.LocalName, ee.Value);    
                }

                inputs.Add(input);
            }

            return inputs;
        }
    }
}
