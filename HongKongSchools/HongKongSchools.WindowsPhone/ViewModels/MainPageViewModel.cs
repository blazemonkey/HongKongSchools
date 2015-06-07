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
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;

        private ObservableCollection<Category> _categories;
        private ObservableCollection<School> _schools;

        private bool _isLoading;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            private set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        public ObservableCollection<School> Schools
        {
            get { return _schools; }
            private set
            {
                _schools = value;
                OnPropertyChanged("Schools");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public DelegateCommand TapSettingsCommand { get; set; }
        public DelegateCommand<School> TapSchoolCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db, INavigationService nav)
        {
            _db = db;
            _nav = nav;

            Categories = new ObservableCollection<Category>();
            Schools = new ObservableCollection<School>();

            TapSettingsCommand = new DelegateCommand(ExecuteTapSettingsCommand);
            TapSchoolCommand = new DelegateCommand<School>(ExecuteTapSchoolCommand);
        }

        private void PopulateSchools(IEnumerable<School> schools)
        {
            Schools.Clear();
            foreach (var school in schools)
                Schools.Add(school);
        }

        public void ExecuteTapSettingsCommand()
        {
            _nav.Navigate(Experiences.Settings, null);
        }

        public void ExecuteTapSchoolCommand(School school)
        {
            _nav.Navigate(Experiences.School, school);
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            IsLoading = true;

            if (viewModelState.Any(x => x.Key == "Schools"))
            {
                if (SettingsPageViewModel.ReloadRequired)
                {
                    var schools = await _db.GetSchools();
                    PopulateSchools(schools);
                    SettingsPageViewModel.ReloadRequired = false;
                }
                else
                {
                    var schools = viewModelState.First(x => x.Key == "Schools").Value as ObservableCollection<School>;
                    Schools = schools;
                }
            }
            else
            {
                var schools = await _db.GetSchools();
                PopulateSchools(schools);
            }

            IsLoading = false;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (viewModelState.Any(x => x.Key == "Schools"))
                viewModelState.Remove("Schools");

            viewModelState.Add("Schools", Schools);
            base.OnNavigatedFrom(viewModelState, suspending);
        }
    }
}
