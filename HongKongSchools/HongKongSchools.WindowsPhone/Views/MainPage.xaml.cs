using HongKongSchools.Controls;
using HongKongSchools.Helpers;
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
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HongKongSchools.Views
{
    public sealed partial class MainPage : PageBase
    {
        private IMessengerService _msg;
        private SolidColorBrush _gpsStatusColor;

        public MainPage()
        {
            this.InitializeComponent();
            LoadingStoryboard.Begin();

            _msg = App.Container.GetInstance<MessengerService>();
            _msg.Register<Geopoint>(this, "PositionChanged", x => DrawPositionChanged(x));
            _msg.Register<PositionStatus>(this, "StatusChanged", x => DrawStatusChanged(x));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _msg.Unregister<Geopoint>(this, "PositionChanged", x => DrawPositionChanged(x));
            _msg.Unregister<PositionStatus>(this, "StatusChanged", x => DrawStatusChanged(x));
            base.OnNavigatedFrom(e);
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
            var center = new BasicGeoposition();
            center.Longitude = geopoint.Position.Longitude;
            center.Latitude = geopoint.Position.Latitude;

            AddPushpin(center);
        }

        public async void AddPushpin(BasicGeoposition location)
        {
            MapControl.Children.Clear();

            var pin = new Grid()
            {
                Width = 30,
                Height = 30,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Name = "GPS",
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

    }
}
