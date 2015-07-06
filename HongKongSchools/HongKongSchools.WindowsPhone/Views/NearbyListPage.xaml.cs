using HongKongSchools.Controls;
using HongKongSchools.Models;
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
        public NearbyListPage()
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
                school.DisplayOrder = index++;
            }

            var grouped = schools.GroupBy(x => x.Level.Name)
                            .OrderBy(x => x.Key);

            schoolsCvs.Source = grouped;     
            base.OnNavigatedTo(e);
        }

    }
}
