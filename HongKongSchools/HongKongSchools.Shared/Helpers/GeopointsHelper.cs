using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Geolocation;

namespace HongKongSchools.Helpers
{
    public static class GeopointsHelper
    {
        private static BasicGeoposition GetAtDistanceBearing(this BasicGeoposition point,
                                                         double distance, double bearing)
        {
            const double degreesToRadian = Math.PI / 180.0;
            const double radianToDegrees = 180.0 / Math.PI;
            const double earthRadius = 6378137.0;

            var latA = point.Latitude * degreesToRadian;
            var lonA = point.Longitude * degreesToRadian;
            var angularDistance = distance / earthRadius;
            var trueCourse = bearing * degreesToRadian;

            var lat = Math.Asin(
                Math.Sin(latA) * Math.Cos(angularDistance) +
                Math.Cos(latA) * Math.Sin(angularDistance) * Math.Cos(trueCourse));

            var dlon = Math.Atan2(
                Math.Sin(trueCourse) * Math.Sin(angularDistance) * Math.Cos(latA),
                Math.Cos(angularDistance) - Math.Sin(latA) * Math.Sin(lat));

            var lon = ((lonA + dlon + Math.PI) % (Math.PI * 2)) - Math.PI;

            var result = new BasicGeoposition();
            result.Latitude = lat * radianToDegrees;
            result.Longitude = lon * radianToDegrees;
            return result;
        }

        public static IList<BasicGeoposition> GetCirclePoints(this BasicGeoposition center,
                                   double radius, int nrOfPoints = 50)
        {
            var angle = 360.0 / nrOfPoints;
            var locations = new List<BasicGeoposition>();
            for (var i = 0; i <= nrOfPoints; i++)
            {
                locations.Add(center.GetAtDistanceBearing(radius, angle * i));
            }
            return locations;
        }
    }
}
