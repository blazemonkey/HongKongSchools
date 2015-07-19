using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.AppDataService;
using HongKongSchools.Services.MessengerService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class SchoolPageViewModel : ViewModel, ISchoolPageViewModel
    {
        private IMessengerService _msg;
        private ISqlLiteService _sql;
        private IAppDataService _appData;
        private DataTransferManager _dataTransferManager;

        private School _selectedSchool;

        public School SelectedSchool
        {
            get { return _selectedSchool; }
            private set
            {
                _selectedSchool = value;
                OnPropertyChanged("SelectedSchool");
            }
        }

        public DelegateCommand<School> CallCommand { get; set; }
        public DelegateCommand<School> OpenWebsiteCommand { get; set; }
        public DelegateCommand ShareCommand { get; set; }
        public DelegateCommand<School> TapCenterMapCommand { get; set; }

        public SchoolPageViewModel(IMessengerService msg, ISqlLiteService sql, IAppDataService appData)
        {
            _msg = msg;
            _sql = sql;
            _appData = appData;

            CallCommand = new DelegateCommand<School>(ExecuteCallCommand);
            OpenWebsiteCommand = new DelegateCommand<School>(ExecuteOpenWebsiteCommand);
            ShareCommand = new DelegateCommand(ExecuteShareCommand);
            TapCenterMapCommand = new DelegateCommand<School>(ExecuteTapCenterMapCommand);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareTextHandler);
        }

        private void ExecuteCallCommand(School school)
        {
            if (string.IsNullOrEmpty(school.Telephone))
                return;

            var telephone = "";
            if (school.Telephone.Split(',').Count() > 1)
            {
                telephone = school.Telephone.Split(',').First();
            }
            else
            {
                telephone = school.Telephone;
            }

            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(telephone, school.SchoolName.SchoolName);
        }

        private async void ExecuteOpenWebsiteCommand(School school)
        {
            if (string.IsNullOrEmpty(school.Website))
                return;

            await Launcher.LaunchUriAsync(new Uri(school.Website));
        }

        private void ExecuteShareCommand()
        {
            DataTransferManager.ShowShareUI();
        }

        public void ExecuteTapCenterMapCommand(School school)
        {
            _msg.Send<School>(school, "ResetZoomLevel");
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            _dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareTextHandler);
            base.OnNavigatedFrom(viewModelState, suspending);
        } 

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var schoolId = _appData.GetSettingsKeyValue<int>("SchoolsPageSchool");
            var selectedSchool = await _sql.GetSchoolById(schoolId);

            SelectedSchool = selectedSchool;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            var request = e.Request;
            var sb = new StringBuilder();
            sb.AppendLine(SelectedSchool.SchoolName.SchoolName);
            sb.AppendLine("");
            sb.AppendLine(SelectedSchool.Address.Name);
            sb.AppendLine(SelectedSchool.District.Name);
            sb.AppendLine(SelectedSchool.Level.Name);
            sb.AppendLine(SelectedSchool.Gender.Name);
            sb.AppendLine(SelectedSchool.FinanceType.Name);
            sb.AppendLine(SelectedSchool.Telephone);
            sb.AppendLine(SelectedSchool.Website);

            // The Title is mandatory
            request.Data.Properties.Title = "Hong Kong Schools";
            request.Data.Properties.Description = "An Hong Kong School's details";

            request.Data.SetText(sb.ToString());
        }
    }
}
