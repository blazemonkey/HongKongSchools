using HongKongSchools.Controls;
using HongKongSchools.Models;
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
        public SchoolPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var selectedSchool = e.Parameter as School;

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
    }
}
