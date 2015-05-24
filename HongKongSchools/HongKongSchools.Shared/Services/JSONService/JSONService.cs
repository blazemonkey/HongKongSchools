using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.JSONService
{
    public class JSONService : IJSONService
    {
        public T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object Deserialize(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public string Serialize(object instance)
        {
            return JsonConvert.SerializeObject(instance);
        }
    }
}
