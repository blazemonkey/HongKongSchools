using HongKongSchools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HongKongSchools
{
    public static class NavigationSettings
    {
        public static IQueryable<School> ResultsPage;
        public static IQueryable<School> NearbyPage;
    }
}
