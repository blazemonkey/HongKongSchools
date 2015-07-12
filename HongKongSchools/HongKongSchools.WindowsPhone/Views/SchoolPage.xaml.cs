using HongKongSchools.Controls;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.SqlLiteService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace HongKongSchools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SchoolPage : PageBase
    {
        private IMessengerService _msg;
        private ISqlLiteService _db;
        private IAppDataService _appData;

        public SchoolPage()
        {
            this.InitializeComponent();

            _msg = App.Container.GetInstance<MessengerService>();
            _db = App.Container.GetInstance<SqlLiteService>();
            _appData = App.Container.GetInstance<AppDataService>();
            _msg.Register<School>(this, "ResetZoomLevel", x => ResetZoomLevel(x));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    AppBarCall.Visibility = Visibility.Visible;
                    AppBarWebsite.Visibility = Visibility.Visible;
                    AppBarShare.Visibility = Visibility.Visible;
                    AppBarCenterMap.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    AppBarCall.Visibility = Visibility.Collapsed;
                    AppBarWebsite.Visibility = Visibility.Collapsed;
                    AppBarShare.Visibility = Visibility.Collapsed;
                    AppBarCenterMap.Visibility = Visibility.Visible;
                    break;
            }

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var schoolId = _appData.GetKeyValue<int>("SchoolsPageSchool");
            var selectedSchool = await _db.GetSchoolById(schoolId);

            if (selectedSchool.Geopoint == null)
                return;

            var position = new BasicGeoposition();
            position.Longitude = selectedSchool.Geopoint.Position.Longitude;
            position.Latitude = selectedSchool.Geopoint.Position.Latitude;
            AddNearbySchool(position);
        }

        private async void AddNearbySchool(BasicGeoposition location)
        {
            var school = new Grid()
            {
                Width = 30,
                Height = 30,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            school.Children.Add(new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/SchoolIcon.png")),
                Width = 30,
                Height = 30
            });

            MapControl.Children.Add(school);
            var position = new Geopoint(location);
            MapControl.SetLocation(school, position);
            await MapControl.TrySetViewAsync(position);    
        }

        private async void ResetZoomLevel(School school)
        {
            var position = new BasicGeoposition();
            position.Longitude = school.Geopoint.Position.Longitude;
            position.Latitude = school.Geopoint.Position.Latitude;

            var center = new Geopoint(position);
            await MapControl.TrySetViewAsync(center, 18);
        }
    }
}
