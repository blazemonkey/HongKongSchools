using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.Services.AppDataService
{
    public interface IAppDataService
    {
        void UpdateSettingsKeyValue<T>(string key, T value);
        T GetSettingsKeyValue<T>(string key);
        void UpdateSettingsLocalFolder(string fileName, string value);
        Task<string> GetSettingsLocalFolder(string fileName);
    }
}
