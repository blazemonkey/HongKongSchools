using System;
using Microsoft.Practices.Unity;
using HongKongSchools.DataParser.Services.XMLReaderService;
using HongKongSchools.DataParser.Services.ExcelReaderService;
using HongKongSchools.DataParser.Services.JSONService;
using HongKongSchools.DataParser.Services.WebClientService;

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
            container.RegisterType<IWebClientService, WebClientService>();
        }

        private static void ResolveTypes(IUnityContainer container)
        {
            //var parser = container.Resolve<Parser>();
            var updater = container.Resolve<Updater>();

            //parser.BeginParse();
            updater.BeginUpdate();
        }

    }
}

