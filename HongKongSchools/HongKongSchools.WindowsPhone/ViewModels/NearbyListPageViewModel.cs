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


        public DelegateCommand<SchoolsWithIndex> TapSchoolCommand { get; set; }
        public DelegateCommand<SchoolsWithIndex> CallCommand { get; set; }

        public NearbyListPageViewModel(ISqlLiteService db, INavigationService nav)
        {
            _db = db;
            _nav = nav;

            TapSchoolCommand = new DelegateCommand<SchoolsWithIndex>(ExecuteTapSchoolCommand);
            CallCommand = new DelegateCommand<SchoolsWithIndex>(ExecuteCallCommand);
        }

        public async void ExecuteTapSchoolCommand(SchoolsWithIndex school)
        {
            var fullSchool = await _db.GetSchoolById(school.School.Id);
            _nav.Navigate(Experiences.School, fullSchool);
        }

        private void ExecuteCallCommand(SchoolsWithIndex school)
        {
            if (string.IsNullOrEmpty(school.School.Telephone))
                return;

            var original = school.School.Telephone;
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(original, school.School.SchoolName.SchoolName);
        }
    }

    public class SchoolsWithIndex
    {
        public School School { get; set; }
        public int Index { get; set; }
    }
}
