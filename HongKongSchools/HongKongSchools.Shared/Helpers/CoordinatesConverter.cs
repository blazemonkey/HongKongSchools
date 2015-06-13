using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace HongKongSchools.Helpers
{
    public class CoordinatesConverter
    {
        public static Geopoint DMSToDDGeopoint(string latitude, string longitude)
        {
            return new Geopoint(new BasicGeoposition() { Latitude = DMSToDD(latitude), Longitude = DMSToDD(longitude) });
        }

        private static double DMSToDD(string coordinate)
        {
            var coordinateDMS = coordinate.Split('-');
            var coordinateDegrees = double.Parse(coordinateDMS[0]);
            var coordinateMinutes = double.Parse(coordinateDMS[1]);
            var coordinateSeconds = double.Parse(coordinateDMS[2]);

            return coordinateDegrees + (coordinateMinutes / 60) + (coordinateSeconds / 3600);
        }
    }
}
