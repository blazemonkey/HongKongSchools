﻿using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.JSONService;
using HongKongSchools.Services.MessageDialogService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.NavigationService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class MainPageViewModel : ViewModel, IMainPageViewModel
    {
        private ISqlLiteService _db;
        private INavigationService _nav;
        private IMessengerService _msg;
        private IAppDataService _appData;
        private IJSONService _json;
        private IMessageDialogService _dialog;

        private string _searchText;
        private ObservableCollection<School> _schools;
        private ObservableCollection<School> _favourites;
        private bool _hasFavourites;

        private ObservableCollection<District> _districts;
        private District _selectedDistrict;
        private ObservableCollection<Level> _levels;
        private Level _selectedLevel;
        private ObservableCollection<FinanceType> _financeTypes;
        private FinanceType _selectedFinanceType;

        private bool _isLoading;
        private Geolocator _geolocator;
        private PositionStatus _positionStatus;
        private Geopoint _geopointSelf;
        private double _latitudeSelf;
        private double _longitudeSelf;

        private IEnumerable<School> _nearbySchools;

        private IDisposable _statusChanged;
        private IDisposable _positionChanged;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
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

        public ObservableCollection<School> Favourites
        {
            get { return _favourites; }
            private set
            {
                _favourites = value;
                OnPropertyChanged("Favourites");
            }
        }

        public bool HasFavourites
        {
            get { return !Favourites.Any();  }
        }

        public ObservableCollection<District> Districts
        {
            get { return _districts; }
            private set
            {
                _districts = value;
                OnPropertyChanged("Districts");
                if (!Districts.Any())
                {
                    Districts.Add(District.CreateAnyOption());
                }
            }
        }

        public District SelectedDistrict
        {
            get { return _selectedDistrict; }
            set
            {
                _selectedDistrict = value;
                OnPropertyChanged("SelectedDistrict");
            }
        }

        public ObservableCollection<Level> Levels
        {
            get { return _levels; }
            private set
            {
                _levels = value;
                OnPropertyChanged("Levels");
                if (!Levels.Any())
                {
                    Levels.Add(Level.CreateAnyOption());
                }
            }
        }

        public Level SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                _selectedLevel = value;
                OnPropertyChanged("SelectedLevel");
            }
        }

        public ObservableCollection<FinanceType> FinanceTypes
        {
            get { return _financeTypes; }
            private set
            {
                _financeTypes = value;
                OnPropertyChanged("FinanceTypes");
                if (!FinanceTypes.Any())
                {
                    FinanceTypes.Add(FinanceType.CreateAnyOption());
                }
            }
        }

        public FinanceType SelectedFinanceType
        {
            get { return _selectedFinanceType; }
            set
            {
                _selectedFinanceType = value;
                OnPropertyChanged("SelectedFinanceType");
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

        public Geolocator Geolocator
        {
            get { return _geolocator; }
            private set
            {
                _geolocator = value;
                OnPropertyChanged("Geolocator");
            }
        }

        public PositionStatus PositionStatus
        {
            get { return _positionStatus; }
            private set
            {
                _positionStatus = value;
                OnPropertyChanged("PositionStatus");
            }
        }

        public Geopoint GeopointSelf
        {
            get { return _geopointSelf; }
            private set
            {
                _geopointSelf = value;
                OnPropertyChanged("GeopointSelf");
            }
        }

        public double LatitudeSelf
        {
            get { return _latitudeSelf; }
            private set
            {
                _latitudeSelf = value;
                OnPropertyChanged("LatitudeSelf");
            }
        }

        public double LongitudeSelf
        {
            get { return _longitudeSelf; }
            private set
            {
                _longitudeSelf = value;
                OnPropertyChanged("LongitudeSelf");
            }
        }

        public IEnumerable<School> NearbySchools
        {
            get { return _nearbySchools; }
            private set
            {
                _nearbySchools = value;
                OnPropertyChanged("NearbySchools");
            }
        }

        public IDisposable StatusChanged
        {
            get { return _statusChanged; }
            private set
            {
                _statusChanged = value;
                OnPropertyChanged("StatusChanged");
            }
        }

        public IDisposable PositionChanged
        {
            get { return _positionChanged; }
            private set
            {
                _positionChanged = value;
                OnPropertyChanged("PositionChanged");
            }
        }

        public DelegateCommand TapSearchSchoolsCommand { get; set; }
        public DelegateCommand TapSettingsCommand { get; set; }
        public DelegateCommand<School> TapSchoolCommand { get; set; }
        public DelegateCommand TapNearbyCommand { get; set; }
        public DelegateCommand TapCenterMapCommand { get; set; }
        public DelegateCommand<School> UnfavouritesCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db, INavigationService nav, IMessengerService msg, 
            IAppDataService appData, IJSONService json, IMessageDialogService dialog)
        {
            _db = db;
            _nav = nav;
            _msg = msg;
            _appData = appData;
            _json = json;
            _dialog = dialog;

            Schools = new ObservableCollection<School>();
            Districts = new ObservableCollection<District>();
            Levels = new ObservableCollection<Level>();
            FinanceTypes = new ObservableCollection<FinanceType>();

            TapSearchSchoolsCommand = new DelegateCommand(ExecuteTapSearchSchoolsCommand);
            TapSettingsCommand = new DelegateCommand(ExecuteTapSettingsCommand);
            TapSchoolCommand = new DelegateCommand<School>(ExecuteTapSchoolCommand);
            TapNearbyCommand = new DelegateCommand(ExecuteTapNearbyCommand);
            TapCenterMapCommand = new DelegateCommand(ExecuteTapCenterMapCommand);
            UnfavouritesCommand = new DelegateCommand<School>(ExecuteUnfavouritesCommand);

            _msg.Register<School>(this, "TapSchool", TapSchool);
        }

        private void Dispose()
        {
            StatusChanged.Dispose();
            PositionChanged.Dispose();

            _msg.Unregister<School>(this, "TapSchool", TapSchool);
        }

        private async Task PopulateSearchBoxes()
        {
            var districts = await _db.GetDistricts();
            var levels = await _db.GetLevels();
            var financeTypes = await _db.GetFinanceTypes();

            foreach (var district in districts)
                Districts.Add(district);

            foreach (var level in levels)
                Levels.Add(level);

            foreach (var financeType in financeTypes)
                FinanceTypes.Add(financeType);

            SelectedDistrict = Districts.First();
            SelectedLevel = Levels.First();
            SelectedFinanceType = FinanceTypes.First();
        }

        private void PopulateSchools(IEnumerable<School> schools)
        {
            Schools.Clear();
            foreach (var school in schools)
                Schools.Add(school);
        }

        private void InitializeGeolocator()
        {
            Geolocator = new Geolocator();
            Geolocator.DesiredAccuracy = PositionAccuracy.High;
            Geolocator.MovementThreshold = 100;

            StatusChanged = Observable.FromEventPattern<StatusChangedEventArgs>(Geolocator, "StatusChanged")
                .ObserveOnDispatcher()
                .Do(x => UpdateStatusChanged(x)).Subscribe();
            PositionChanged = Observable.FromEventPattern<PositionChangedEventArgs>(Geolocator, "PositionChanged")
                .ObserveOnDispatcher()
                .Do(x => UpdatePositionChanged(x)).Subscribe();       
        }

        private void UpdateStatusChanged(EventPattern<StatusChangedEventArgs> e)
        {
            System.Diagnostics.Debug.WriteLine(e.EventArgs.Status);
            PositionStatus = e.EventArgs.Status;

            if (PositionStatus == PositionStatus.Ready)
            {
                _msg.Send<PositionStatus>(PositionStatus, "StatusChanged");
                return;
            }
            
            LongitudeSelf = _appData.GetSettingsKeyValue<double>("LastPositionLongitude");
            LatitudeSelf = _appData.GetSettingsKeyValue<double>("LastPositionLatitude");
            
            var basicGeoposition = new BasicGeoposition();
            basicGeoposition.Latitude = LatitudeSelf;
            basicGeoposition.Longitude = LongitudeSelf;
            GeopointSelf = new Geopoint(basicGeoposition);

            _msg.Send<PositionStatus>(PositionStatus, "StatusChanged");
            _msg.Send<Geopoint>(GeopointSelf, "PositionChanged");
        }

        private void UpdatePositionChanged(EventPattern<PositionChangedEventArgs> e)
        {
            GeopointSelf = new Geopoint(e.EventArgs.Position.Coordinate.Point.Position);
            LatitudeSelf = e.EventArgs.Position.Coordinate.Point.Position.Latitude;
            LongitudeSelf = e.EventArgs.Position.Coordinate.Point.Position.Longitude;
            System.Diagnostics.Debug.WriteLine(string.Format("Latitude: {0}, Longitude: {1}",
                GeopointSelf.Position.Latitude,
                GeopointSelf.Position.Longitude));

            NearbySchools = SchoolsWithinsSquare(GeopointSelf);
            foreach (var school in NearbySchools)
            {
                System.Diagnostics.Debug.WriteLine(school.SchoolName.SchoolName);
            }

            _appData.UpdateSettingsKeyValue<double>("LastPositionLongitude", LongitudeSelf);
            _appData.UpdateSettingsKeyValue<double>("LastPositionLatitude", LatitudeSelf);

            _msg.Send<Geopoint>(GeopointSelf, "PositionChanged");
            _msg.Send<IEnumerable<School>>(NearbySchools, "NearbySchoolsChanged");
        }

        public async void ExecuteTapSearchSchoolsCommand()
        {
            if (string.IsNullOrEmpty(SearchText) && SelectedDistrict.DistrictId == 0 && SelectedFinanceType.FinanceTypeId == 0
                && SelectedLevel.LevelId == 0)
            {
                var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
                var localizedText = resourceLoader.GetString("search_error_message");

                await _dialog.Show(localizedText);
                return;
            }

            var query = Schools.AsQueryable();

            if (SelectedDistrict.DistrictId > 0)
            {
                query = query.Where(x => x.DistrictId == SelectedDistrict.DistrictId);
            }

            if (SelectedFinanceType.FinanceTypeId > 0)
            {
                query = query.Where(x => x.FinanceTypeId == SelectedFinanceType.FinanceTypeId);
            }

            if (SelectedLevel.LevelId > 0)
            {
                query = query.Where(x => x.LevelId == SelectedLevel.LevelId);
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                query = query.Where(x => x.SchoolName.SchoolName.Contains(SearchText.ToUpper()));
            }

            NavigationSettings.ResultsPage = query;
            _nav.Navigate(Experiences.Results);
        }

        public void ExecuteTapSettingsCommand()
        {
            _nav.Navigate(Experiences.Settings, null);
        }

        public void ExecuteTapSchoolCommand(School school)
        {
            _appData.UpdateSettingsKeyValue<int>("SchoolsPageSchool", school.Id);
            _nav.Navigate(Experiences.School);
        }

        public void ExecuteTapNearbyCommand()
        {
            if (NearbySchools == null)
                NavigationSettings.NearbyPage = null;
            else
                NavigationSettings.NearbyPage = NearbySchools.AsQueryable();

            _nav.Navigate(Experiences.NearbyList);
        }

        public void ExecuteTapCenterMapCommand()
        {
            if (Geolocator.LocationStatus == PositionStatus.Ready)
            {
                _msg.Send<Geopoint>(GeopointSelf, "ResetZoomLevel");
            }
        }

        public async void ExecuteUnfavouritesCommand(School school)
        {
            var resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            var localizedText = resourceLoader.GetString("confirm_unfavourite_message");

            await _dialog.ShowYesNo(string.Format(localizedText, school.SchoolName.SchoolName), () =>
                {
                    school.IsFavourite = false;
                    _db.UpdateFavourites(school);
                    Favourites.Remove(school);
                }
                );
        }

        public async void TapSchool(School school)
        {
            var fullSchool = await _db.GetSchoolById(school.Id);
            _nav.Navigate(Experiences.School, fullSchool);
        }

        private IEnumerable<School> SchoolsWithinsSquare(Geopoint center)
        {
            var schools = Schools.Where(x => x.Geopoint != null)
                        .Where(x => (x.Geopoint.Position.Longitude <= center.Position.Longitude + 0.0025) &&
                        (x.Geopoint.Position.Longitude >= center.Position.Longitude - 0.0025) &&
                        (x.Geopoint.Position.Latitude <= center.Position.Latitude + 0.0025) &&
                        (x.Geopoint.Position.Latitude >= center.Position.Latitude - 0.0025));            
             
            return schools.OrderBy(x => GetNearestSchool(x));
        }

        private double GetNearestSchool(School school)
        {
            var latitude = school.Geopoint.Position.Latitude - LatitudeSelf;
            var longitude = school.Geopoint.Position.Longitude - LongitudeSelf;

            latitude = latitude < 0 ? latitude * -1 : latitude;
            longitude = longitude < 0 ? longitude * -1 : longitude;

            return latitude + longitude;
        }

        private void Favourites_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("HasFavourites");
            if (e.Action == NotifyCollectionChangedAction.Reset)
                return;

            if (!Favourites.Any())
                return;

            var index = 1;
            foreach (var f in Favourites)
                f.DisplayOrder = index++;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (Geolocator == null)
                InitializeGeolocator();

            IsLoading = true;

            if (!Schools.Any())
            {
                await PopulateSearchBoxes();
                var schools = await _db.GetSchools();
                PopulateSchools(schools);
            }

            Favourites = new ObservableCollection<School>();
            Favourites.CollectionChanged -= Favourites_CollectionChanged;
            Favourites.CollectionChanged += Favourites_CollectionChanged;
            foreach (var s in Schools.Where(x => x.IsFavourite))
                Favourites.Add(s);

            IsLoading = false;

            if (Geolocator.LocationStatus == PositionStatus.Ready)
            {
                NearbySchools = SchoolsWithinsSquare(GeopointSelf);
                _msg.Send<Geopoint>(GeopointSelf, "PositionChanged");
                _msg.Send<IEnumerable<School>>(NearbySchools, "DrawNearbySchoolsChanged");
            }
            
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
