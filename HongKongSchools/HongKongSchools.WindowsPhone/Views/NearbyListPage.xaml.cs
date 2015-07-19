using HongKongSchools.Controls;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.JSONService;
using HongKongSchools.Services.SqlLiteService;
using HongKongSchools.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace HongKongSchools.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NearbyListPage : PageBase
    {
        private IJSONService _json;
        private ISqlLiteService _db;
        private IAppDataService _appData;

        public NearbyListPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

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

            SearchingStackPanel.Visibility = Visibility.Visible;
            SchoolsListTextBlock.Visibility = Visibility.Collapsed;
            SchoolsListView.Visibility = Visibility.Collapsed;

            var schoolsJson = _appData.GetSettingsKeyValue<string>("NearbyPageSchools");
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
            }

            var grouped = schools.GroupBy(x => x.Level.Name)
                            .OrderBy(x => x.Key);

            schoolsCvs.Source = grouped;
            SchoolsListView.ScrollIntoView(schools.FirstOrDefault());

            SearchingStackPanel.Visibility = Visibility.Collapsed;
            SchoolsListTextBlock.Visibility = Visibility.Collapsed;
            SchoolsListView.Visibility = Visibility.Visible;
        }

    }
}
