using HongKongSchools.Interfaces;
using HongKongSchools.Models;
using HongKongSchools.Services.NavigationService;
using HongKongSchools.Services.SqlLiteService;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;

namespace HongKongSchools.ViewModels
{
    public class SettingsPageViewModel : ViewModel, ISettingsPageViewModel
    {
        public static bool ReloadRequired { get; set; }

        private ISqlLiteService _db;
        private INavigationService _nav;
        private ObservableCollection<Language> _languages;
        private Language _selectedLanguage;
        private int _initialLanguageId;
        private string _version;

        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                OnPropertyChanged("Languages");
            }
        }

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");
                UpdateLanguage(_selectedLanguage.Culture);
            }
        }

        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChanged("Version");
            }
        }

        public SettingsPageViewModel(ISqlLiteService db, INavigationService nav)
        {
            _db = db;
            _nav = nav;

            Languages = new ObservableCollection<Language>();
        }

        private async Task PopulateLanguages()
        {
            var languages = await _db.GetLanguages();
            foreach (var lang in languages)
                Languages.Add(lang);

            SetLanguage(CultureInfo.CurrentCulture);
            _initialLanguageId = SelectedLanguage.LanguageId;
        }

        private void SetLanguage(CultureInfo ci)
        {
           if (Languages.Any(x => x.Culture == ci.Name))
           {
               SelectedLanguage = Languages.First(x => x.Culture == ci.Name);
               return;
           }

           SelectedLanguage = Languages.First(x => x.Culture == "en");
        }

        private void UpdateLanguage(string culture)
        {
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            await PopulateLanguages();

            var file = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
            var appVersion = XDocument.Load("AppxManifest.xml").Root.Elements().Where(x => x.Name.LocalName == "Identity")
                                .First().Attributes().Where(x => x.Name.LocalName == "Version").First().Value;
            Version = appVersion;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            ReloadRequired = _initialLanguageId == SelectedLanguage.LanguageId ? false : true;
            base.OnNavigatedFrom(viewModelState, suspending);
        }
    }
}
