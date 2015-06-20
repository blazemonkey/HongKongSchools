using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.AppDataService
{
    public interface IAppDataService
    {
        void UpdateKeyValue<T>(string key, T value);
        T GetKeyValue<T>(string key);
    }
}
