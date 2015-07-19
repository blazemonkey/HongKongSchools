using HongKongSchools.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HongKongSchools.Services.AppDataService
{
    public class AppDataService : IAppDataService
    {
        private ApplicationDataContainer _appDataSettings;
        private StorageFolder _appDataTempFolder;

        public AppDataService()
        {
            _appDataSettings = ApplicationData.Current.LocalSettings;
            _appDataTempFolder = ApplicationData.Current.TemporaryFolder;
        }

        public void InitializeAppDataContainer()
        {
            InsertIntoAppDataContainer("LastPositionLongitude", 0.0);
            InsertIntoAppDataContainer("LastPositionLatitude", 0.0);
            InsertIntoAppDataContainer("ResultsPageSchools", "");
            InsertIntoAppDataContainer("NearbyPageSchools", "");
            InsertIntoAppDataContainer("SchoolsPageSchool", 0.0);
        }

        private void InsertIntoAppDataContainer(string key, object value)
        {
            if (!_appDataSettings.Values.ContainsKey(key))
                _appDataSettings.Values.Add(new KeyValuePair<string, object>(key, value));
        }

        public void UpdateSettingsKeyValue<T>(string key, T value)
        {
            if (!_appDataSettings.Values.ContainsKey(key))
                throw new ArgumentException("Key Not Found in App Data Container");

            _appDataSettings.Values[key] = value;
        }

        public T GetSettingsKeyValue<T>(string key)
        {
            if (!_appDataSettings.Values.ContainsKey(key))
                throw new ArgumentException("Key Not Found in App Data Container");

            return (T)_appDataSettings.Values[key];
        }

        public async void UpdateSettingsLocalFolder(string fileName, string value)
        {
            var localFile = await _appDataTempFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(localFile, value);
        }

        public async Task<string> GetSettingsLocalFolder(string fileName)
        {
            var localFile = await _appDataTempFolder.GetFileAsync(fileName);
            var value = await FileIO.ReadTextAsync(localFile);

            return value;
        }
    }
}
