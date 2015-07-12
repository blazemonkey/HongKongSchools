using HongKongSchools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace HongKongSchools.Services.AppDataService
{
    public class AppDataService : IAppDataService
    {
        private ApplicationDataContainer _appDataContainer;

        public AppDataService()
        {
            _appDataContainer = ApplicationData.Current.LocalSettings;
        }

        public void InitializeAppDataContainer()
        {
            InsertIntoAppDataContainer("LastPositionLongitude", 0);
            InsertIntoAppDataContainer("LastPositionLatitude", 0);
            InsertIntoAppDataContainer("ResultsPageSchools", "");
            InsertIntoAppDataContainer("NearbyPageSchools", "");
            InsertIntoAppDataContainer("SchoolsPageSchool", 0);
        }

        private void InsertIntoAppDataContainer(string key, object value)
        {
            if (!_appDataContainer.Values.ContainsKey(key))
                _appDataContainer.Values.Add(new KeyValuePair<string, object>(key, value));
        }

        public void UpdateKeyValue<T>(string key, T value)
        {
            if (!_appDataContainer.Values.ContainsKey(key))
                throw new ArgumentException("Key Not Found in App Data Container");

            _appDataContainer.Values[key] = value;
        }

        public T GetKeyValue<T>(string key)
        {
            if (!_appDataContainer.Values.ContainsKey(key))
                throw new ArgumentException("Key Not Found in App Data Container");

            return (T)_appDataContainer.Values[key];
        }
    }
}
