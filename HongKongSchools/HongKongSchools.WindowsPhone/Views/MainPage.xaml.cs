using HongKongSchools.Controls;
using HongKongSchools.Helpers;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.MessengerService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HongKongSchools.Views
{
    public sealed partial class MainPage : PageBase
    {
        private IMessengerService _msg;
        private IAppDataService _appData;

        private SolidColorBrush _gpsStatusColor;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            LoadingStoryboard.Begin();

            _msg = App.Container.GetInstance<MessengerService>();
            _appData = App.Container.GetInstance<AppDataService>();

            _msg.Register<Geopoint>(this, "PositionChanged", x => DrawPositionChanged(x));
            _msg.Register<PositionStatus>(this, "StatusChanged", x => DrawStatusChanged(x));
            _msg.Register<IEnumerable<School>>(this, "NearbySchoolsChanged", x => DrawNearbySchoolsChanged(x));
            _msg.Register<Geopoint>(this, "ResetZoomLevel", x => ResetZoomLevel(x));
        }

        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    AppBarSetting.Visibility = Visibility.Visible;
                    AppBarNearby.Visibility = Visibility.Collapsed;
                    AppBarCenterMap.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    AppBarSetting.Visibility = Visibility.Collapsed;
                    AppBarNearby.Visibility = Visibility.Visible;
                    AppBarCenterMap.Visibility = Visibility.Visible;
                    break;
                case 2:
                    AppBarSetting.Visibility = Visibility.Visible;
                    AppBarNearby.Visibility = Visibility.Collapsed;
                    AppBarCenterMap.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void DrawStatusChanged(PositionStatus status)
        {
            if (status == PositionStatus.Ready)
            {
                _gpsStatusColor = new SolidColorBrush(Colors.Green);
                //GPSStoryboard.Stop();
                return;
            }

            _gpsStatusColor = new SolidColorBrush(Colors.Red);
            //GPSStoryboard.Begin();
        }

        private void DrawPositionChanged(Geopoint geopoint)
        {
            ClearMapIcons<Ellipse>();

            var center = new BasicGeoposition();
            center.Longitude = geopoint.Position.Longitude;
            center.Latitude = geopoint.Position.Latitude;

            AddPushpin(center);
        }

        private void DrawNearbySchoolsChanged(IEnumerable<School> schools)
        {
            ClearMapIcons<Image>();

            var index = 1;
            foreach (var school in schools)
            {
                var position = new BasicGeoposition();
                position.Longitude = school.Geopoint.Position.Longitude;
                position.Latitude = school.Geopoint.Position.Latitude;

                AddNearbySchool(school, position, index++);
            }
        }

        private async void AddPushpin(BasicGeoposition location)
        {
            var pin = new Grid()
            {
                Width = 30,
                Height = 30,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
                {
                    Fill = _gpsStatusColor,
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 3,
                    Width = 30,
                    Height = 30
                });

            MapControl.Children.Add(pin);
            var center = new Geopoint(location);
            MapControl.SetLocation(pin, center);
            await MapControl.TrySetViewAsync(center);
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

        private async void ResetZoomLevel(Geopoint center)
        {
            await MapControl.TrySetViewAsync(center, 16);            
        }

        private void school_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = (Grid)sender;
            var school = (School)grid.Tag;

            _appData.UpdateSettingsKeyValue<int>("SchoolsPageSchool", school.Id);
            _msg.Send<School>(school, "TapSchool");
        }

    }
}
