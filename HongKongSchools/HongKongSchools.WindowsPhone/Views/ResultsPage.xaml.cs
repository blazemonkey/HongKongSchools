using HongKongSchools.Controls;
using HongKongSchools.Models;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public ResultsPage()
        {
            this.InitializeComponent();
            _msg = App.Container.GetInstance<MessengerService>();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var schools = e.Parameter as IEnumerable<School>;
            if (schools == null || !schools.Any())
            {
                SchoolsListTextBlock.Visibility = Visibility.Visible;
                SchoolsListView.Visibility = Visibility.Collapsed;
                return;
            }

            SchoolsListTextBlock.Visibility = Visibility.Collapsed;
            SchoolsListView.Visibility = Visibility.Visible;

            var index = 1;
            foreach (var school in schools)
            {
                if (school.SchoolName.SchoolName == "優才(楊殷有娣)書院")
                {

                }
                school.DisplayOrder = index++;
               
                if (school.Geopoint == null)
                    continue;

                var position = new BasicGeoposition();
                position.Longitude = school.Geopoint.Position.Longitude;
                position.Latitude = school.Geopoint.Position.Latitude;
                AddNearbySchool(school, position, school.DisplayOrder);
            }         

            var center = GetCenterOfPoints(schools.Where(x => x.Geopoint != null).Select(x => x.Geopoint));
            var centerPos = new Geopoint(center);

            var grouped = schools.GroupBy(x => x.Level.Name)
                            .OrderBy(x => x.Key);

            schoolsCvs.Source = grouped;
            await MapControl.TrySetViewAsync(centerPos);
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

        private BasicGeoposition GetCenterOfPoints(IEnumerable<Geopoint> positions)
        {
            var maxLatitude = positions.Max(x => x.Position.Latitude);
            var minLatitude = positions.Min(x => x.Position.Latitude);

            var maxLongitude = positions.Max(x => x.Position.Longitude);
            var minLongitude = positions.Min(x => x.Position.Longitude);

            var centerLatitude = ((maxLatitude - minLatitude) / 2) + minLatitude;
            var centerLongitude = ((maxLongitude - minLongitude) / 2) + minLongitude;

            var pos = new BasicGeoposition()
            {
                Latitude = centerLatitude,
                Longitude = centerLongitude
            };

            return pos;
        }

        private void school_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var grid = (Grid)sender;
            var school = (School)grid.Tag;
            _msg.Send<School>(school, "TapSchool");
        }
    }
}
