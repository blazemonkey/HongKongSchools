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

namespace HongKongSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;

        private ObservableCollection<Category> _categories;
        private ObservableCollection<School> _schools;

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

        public DelegateCommand TapSettingsCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db, INavigationService nav)
        {
            _db = db;
            _nav = nav;

            Categories = new ObservableCollection<Category>();
            Schools = new ObservableCollection<School>();

            TapSettingsCommand = new DelegateCommand(ExecuteTapSettingsCommand);
        }

        private async Task PopulateCategories()
        {
            Categories.Clear();
            var categories = await _db.GetCategories();
            foreach (var category in categories)
                Categories.Add(category);
        }

        private async Task PopulateSchools()
        {
            Schools.Clear();
            var schools = await _db.GetSchools();
            foreach (var school in schools)
                Schools.Add(school);
        }

        public void ExecuteTapSettingsCommand()
        {
            _nav.Navigate(Experiences.Settings, null);
        }

        public override async void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            await PopulateCategories();
            await PopulateSchools();
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

    }
}
