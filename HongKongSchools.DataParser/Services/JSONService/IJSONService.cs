using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Services.JSONService
{
    public interface IJSONService
    {
        string Serialize(object value);
        T Deserialize<T>(string value);
    }
}
