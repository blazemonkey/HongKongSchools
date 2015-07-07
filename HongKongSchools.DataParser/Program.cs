using System;
using Microsoft.Practices.Unity;
using HongKongSchools.DataParser.Services.XMLReaderService;
using HongKongSchools.DataParser.Services.ExcelReaderService;
using HongKongSchools.DataParser.Services.JSONService;

namespace HongKongSchools.DataParser
{
    class Program
    {
        static void Main()
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

