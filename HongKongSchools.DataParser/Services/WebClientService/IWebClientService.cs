using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Services.WebClientService
{
    public interface IWebClientService
    {
        void DownloadFile(string url, string storeLocation);
    }
}
