using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.JSONService
{
    public interface IJSONService
    {
        T Deserialize<T>(string json);
        object Deserialize(string json);
        string Serialize(object instance);
    }
}
