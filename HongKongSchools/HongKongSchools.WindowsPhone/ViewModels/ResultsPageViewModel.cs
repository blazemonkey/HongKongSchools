using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.JSONService;
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
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class ResultsPageViewModel : ViewModel, IResultsPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;
        private IAppDataService _appData;

        public DelegateCommand<School> TapSchoolCommand { get; set; }
        public DelegateCommand<School> FavouritesCommand { get; set; }

        public ResultsPageViewModel(ISqlLiteService db, INavigationService nav, IAppDataService appData)
        {
            _db = db;
            _nav = nav;
            _appData = appData;

            TapSchoolCommand = new DelegateCommand<School>(ExecuteTapSchoolCommand);
            FavouritesCommand = new DelegateCommand<School>(ExecuteFavouritesCommand);
        }

        public void ExecuteTapSchoolCommand(School school)
        {
            _appData.UpdateSettingsKeyValue<int>("SchoolsPageSchool", school.Id);
            _nav.Navigate(Experiences.School);
        }

        private async void ExecuteFavouritesCommand(School school)
        {
            school.IsFavourite = !school.IsFavourite;
            await _db.UpdateFavourites(school);
        }
    }
}
