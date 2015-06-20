using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.FileReaderService;
using HongKongSchools.Services.JSONService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.NavigationService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Mvvm;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace HongKongSchools
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public static readonly Container Container = new Container();
        public static double ScrollOffset;

        public App()
        {
            this.InitializeComponent();
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterSingle(NavigationService);
            Container.Register<IAppDataService, AppDataService>();
            Container.Register<INavigationService, NavigationService>();
            Container.Register<IMessengerService, MessengerService>();
            Container.Register<IFileReaderService, FileReaderService>();
            Container.Register<IJSONService, JSONService>();
            Container.Register<ISqlLiteService, SqlLiteService>();

            await Container.GetInstance<SqlLiteService>().ClearLocalDb();
            Container.GetInstance<AppDataService>().InitializeAppDataContainer();
            return;
        }

        protected override object Resolve(Type type)
        {
            return Container.GetInstance(type);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs e)
        {
            NavigationService.Navigate(Experiences.Main.ToString(), null);
            return Task.FromResult<object>(null);
        }
    }
}