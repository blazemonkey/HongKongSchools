using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using HongKongSchools.DataParser.Models;
using Newtonsoft.Json;
using System.Xml.Linq;
using HongKongSchools.DataParser.Helpers;
using Microsoft.Practices.Unity;
using HongKongSchools.DataParser.Services.XMLReaderService;
using HongKongSchools.DataParser.Services.ExcelReaderService;
using HongKongSchools.DataParser.Services.JSONService;

namespace HongKongSchools.DataParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            ResolveTypes(container);

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IXMLReaderService, XMLReaderService>();
            container.RegisterType<IExcelReaderService, ExcelReaderService>();
            container.RegisterType<IJSONService, JSONService>();
        }

        private static void ResolveTypes(IUnityContainer container)
        {
            var parser = container.Resolve<Parser>();
            parser.BeginParse();
        }

    }
}

