using HongKongSchools.Controls;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.JSONService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.SqlLiteService;
using HongKongSchools.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class ResultsPage : PageBase
    {
        private IMessengerService _msg;
        private IAppDataService _appData;
        private IJSONService _json;
        private ISqlLiteService _db;

        public ResultsPage()
        {            
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            _msg = App.Container.GetInstance<MessengerService>();
            _appData = App.Container.GetInstance<AppDataService>();
            _json = App.Container.GetInstance<JSONService>();
            _db = App.Container.GetInstance<SqlLiteService>();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {            
            if (e.NavigationMode == NavigationMode.Back)
                return;

            schoolsCvs.Source = null;
            ClearMapIcons<Image>();

            SearchingStackPanel.Visibility = Visibility.Visible;
            SchoolsListTextBlock.Visibility = Visibility.Collapsed;
            SchoolsListView.Visibility = Visibility.Collapsed;
            MainPivot.SelectedIndex = 0;

            var schoolsJson = _appData.GetKeyValue<string>("ResultsPageSchools");
            var schoolsId = _json.Deserialize<IEnumerable<int>>(schoolsJson).ToList();
            var schools = await _db.GetSchoolsByIds(schoolsId);

            if (schools == null || !schools.Any())
            {
                SearchingStackPanel.Visibility = Visibility.Collapsed;
                SchoolsListTextBlock.Visibility = Visibility.Visible;
                SchoolsListView.Visibility = Visibility.Collapsed;
                return;
            }

            var index = 1;
            foreach (var school in schools)
            {
                school.DisplayOrder = index++;

                if (school.Geopoint == null)
                    continue;

                var position = new BasicGeoposition();
                position.Longitude = school.Geopoint.Position.Longitude;
                position.Latitude = school.Geopoint.Position.Latitude;
                AddNearbySchool(school, position, school.DisplayOrder);
            }

            var grouped = schools;
            schoolsCvs.Source = grouped;
            SchoolsListView.ScrollIntoView(schools.FirstOrDefault());

            SearchingStackPanel.Visibility = Visibility.Collapsed;
            SchoolsListTextBlock.Visibility = Visibility.Collapsed;
            SchoolsListView.Visibility = Visibility.Visible;

            await SetCenterOfPoints(schools.Where(x => x.Geopoint != null).Select(x => x.Geopoint));
            MapControl.ZoomLevel = 12;
        }

        private void AddNearbySchool(School school, BasicGeoposition location, int index)
        {
            var grid = new Grid()
            {
                Width = 30,
                Height = 30,
                Margin = new Windows.UI.Xaml.Thickness(-12),
                Tag = school
            };
            grid.Tapped += school_Tapped;
            grid.Children.Add(new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Images/SchoolIcon.png")),
                Width = 30,
                Height = 30
            });

            grid.Children.Add(new TextBlock()
            {
                Text = index.ToString(),
                FontSize = 16,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
            });

            MapControl.Children.Add(grid);
            var position = new Geopoint(location);
            MapControl.SetLocation(grid, position);
        }

        private async Task SetCenterOfPoints(IEnumerable<Geopoint> positions)
        {
            if (positions.Count() == 1)
            {
                await MapControl.TrySetViewAsync(positions.First());
                return;
            }

            var maxLatitude = positions.Max(x => x.Position.Latitude);
            var minLatitude = positions.Min(x => x.Position.Latitude);

            var maxLongitude = positions.Max(x => x.Position.Longitude);
            var minLongitude = positions.Min(x => x.Position.Longitude);

            var centerLatitude = ((maxLatitude - minLatitude) / 2) + minLatitude;
            var centerLongitude = ((maxLongitude - minLongitude) / 2) + minLongitude;

            var nw = new BasicGeoposition()
            {
                Latitude = maxLatitude,
                Longitude = minLongitude
            };

            var se = new BasicGeoposition()
            {
                Latitude = minLatitude,
                Longitude = maxLongitude
            };

            if (maxLongitude != minLongitude && maxLatitude != minLatitude)
            {
                var mapWidth = MapControl.Width;
                var buffer = 1;
                //best zoom level based on map width
                var zoom1 = Math.Log(360.0 / 256.0 * (MapControl.Width - 2*buffer) / (maxLongitude - minLongitude)) / Math.Log(2);
                //best zoom level based on map height
                var zoom2 = Math.Log(180.0 / 256.0 * (MapControl.Height - 2*buffer) / (minLatitude - minLatitude)) / Math.Log(2);
            }

            var box = new GeoboundingBox(nw, se);
            await MapControl.TrySetViewAsync(new Geopoint(box.Center));
        }


        private void ClearMapIcons<T>()
        {
            var types = MapControl.Children.Where(x => x.GetType() == typeof(Grid))
            .Where(x => ((Grid)x).Children.Any(z => z.GetType() == typeof(T))).ToList();

            for (var x = 0; x < types.Count(); x++)
            {
                var grid = (Grid)types.ElementAt(x);
                grid.Tapped -= school_Tapped;
                MapControl.Children.Remove(grid);
            }
        }

        private void school_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = (Grid)sender;
            var school = (School)grid.Tag;

            _appData.UpdateKeyValue<int>("SchoolsPageSchool", school.Id);
            _msg.Send<School>(school, "TapSchool");
        }
    }
}
