using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.NavigationService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class NearbyListPageViewModel : ViewModel, INearbyListPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;

        public DelegateCommand<School> TapSchoolCommand { get; set; }
        public DelegateCommand<School> CallCommand { get; set; }

        public NearbyListPageViewModel(ISqlLiteService db, INavigationService nav)
        {
            _db = db;
            _nav = nav;

            TapSchoolCommand = new DelegateCommand<School>(ExecuteTapSchoolCommand);
            CallCommand = new DelegateCommand<School>(ExecuteCallCommand);
        }

        public async void ExecuteTapSchoolCommand(School school)
        {
            var fullSchool = await _db.GetSchoolById(school.Id);
            _nav.Navigate(Experiences.School, fullSchool);
        }

        private void ExecuteCallCommand(School school)
        {
            if (string.IsNullOrEmpty(school.Telephone))
                return;

            var original = school.Telephone;
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(original, school.SchoolName.SchoolName);
        }
    }

}
