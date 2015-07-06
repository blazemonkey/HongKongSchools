using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.MessengerService;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class SchoolPageViewModel : ViewModel, ISchoolPageViewModel
    {
        private IMessengerService _msg;

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

        public DelegateCommand CallCommand { get; set; }
        public DelegateCommand OpenWebsiteCommand { get; set; }
        public DelegateCommand TapCenterMapCommand { get; set; }

        public SchoolPageViewModel(IMessengerService msg)
        {
            _msg = msg;

            CallCommand = new DelegateCommand(ExecuteCallCommand);
            OpenWebsiteCommand = new DelegateCommand(ExecuteOpenWebsiteCommand);
            TapCenterMapCommand = new DelegateCommand(ExecuteTapCenterMapCommand);
        }

        private void ExecuteCallCommand()
        {
            if (string.IsNullOrEmpty(SelectedSchool.Telephone))
                return;

            var original = SelectedSchool.Telephone;
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(original, SelectedSchool.SchoolName.SchoolName);
        }

        private async void ExecuteOpenWebsiteCommand()
        {
            if (string.IsNullOrEmpty(SelectedSchool.Website))
                return;

            await Launcher.LaunchUriAsync(new Uri(SelectedSchool.Website));
        }

        public void ExecuteTapCenterMapCommand()
        {
            _msg.Send<School>(SelectedSchool, "ResetZoomLevel");
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var selectedSchool = navigationParameter as School;
            SelectedSchool = selectedSchool;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
