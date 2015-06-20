using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.NavigationService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Category> _categories;
        private ObservableCollection<School> _schools;

        private bool _isLoading;
        private Geolocator _geolocator;
        private PositionStatus _positionStatus;
        private Geopoint _geopointSelf;
        private double _latitudeSelf;
        private double _longitudeSelf;

        private IDisposable _statusChanged;
        private IDisposable _positionChanged;

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

        public DelegateCommand TapSettingsCommand { get; set; }
        public DelegateCommand<School> TapSchoolCommand { get; set; }

        public MainPageViewModel(ISqlLiteService db, INavigationService nav, IMessengerService msg, IAppDataService appData)
        {
            _db = db;
            _nav = nav;
            _msg = msg;
            _appData = appData;

            Categories = new ObservableCollection<Category>();
            Schools = new ObservableCollection<School>();

            TapSettingsCommand = new DelegateCommand(ExecuteTapSettingsCommand);
            TapSchoolCommand = new DelegateCommand<School>(ExecuteTapSchoolCommand);
        }

        private void Dispose()
        {
            StatusChanged.Dispose();
            PositionChanged.Dispose();
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
            Geolocator.MovementThreshold = 10;

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
            
            LongitudeSelf = _appData.GetKeyValue<double>("LastPositionLongitude");
            LatitudeSelf = _appData.GetKeyValue<double>("LastPositionLatitude");
            
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

            foreach (var school in SchoolsWithinsSquare(GeopointSelf))
            {
                System.Diagnostics.Debug.WriteLine(school.SchoolName.SchoolName);
            }

            _appData.UpdateKeyValue<double>("LastPositionLongitude", LongitudeSelf);
            _appData.UpdateKeyValue<double>("LastPositionLatitude", LatitudeSelf);

            _msg.Send<Geopoint>(GeopointSelf, "PositionChanged");
        }

        public void ExecuteTapSettingsCommand()
        {
            _nav.Navigate(Experiences.Settings, null);
        }

        public void ExecuteTapSchoolCommand(School school)
        {
            _nav.Navigate(Experiences.School, school);
        }

        private IEnumerable<School> SchoolsWithinsSquare(Geopoint center)
        {
            var schools = Schools.Where(x => (x.Geopoint.Position.Longitude <= center.Position.Longitude + 0.0025) &&
                        (x.Geopoint.Position.Longitude >= center.Position.Longitude - 0.0025) &&
                        (x.Geopoint.Position.Latitude <= center.Position.Latitude + 0.0025) &&
                        (x.Geopoint.Position.Latitude >= center.Position.Latitude - 0.0025));

            return schools;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            InitializeGeolocator();
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
            Dispose();

            if (viewModelState.Any(x => x.Key == "Schools"))
                viewModelState.Remove("Schools");

            viewModelState.Add("Schools", Schools);
            base.OnNavigatedFrom(viewModelState, suspending);
        }
    }
}
