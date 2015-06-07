using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class SchoolPageViewModel : ViewModel, ISchoolPageViewModel
    {
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

        public SchoolPageViewModel()
        {
            CallCommand = new DelegateCommand(ExecuteCallCommand);
        }

        private void ExecuteCallCommand()
        {
            if (string.IsNullOrEmpty(SelectedSchool.Telephone))
                return;

            var original = SelectedSchool.Telephone;
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(original, SelectedSchool.Address.Name);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var selectedSchool = navigationParameter as School;
            SelectedSchool = selectedSchool;
        }
    }
}
