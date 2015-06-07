using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.NavigationService
{
    public enum Experiences { Main, Settings, School }

    public interface INavigationService
    {
        bool Navigate(Experiences experience, object param = null);
        void GoBack();
        bool CanGoBack { get; }
        void ClearHistory();
    }
}
