using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.NavigationService
{
    public enum Experiences { Main, Settings, School, NearbyList, Results }

    public interface INavigationService
    {
        bool Navigate(Experiences experience, object param = null);
        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
