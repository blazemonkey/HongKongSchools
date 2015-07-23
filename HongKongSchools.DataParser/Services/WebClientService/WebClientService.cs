using System;
using System.Net;

namespace HongKongSchools.DataParser.Services.WebClientService
{
    public class WebClientService : IWebClientService
    {
        public void DownloadFile(string url, string storeLocation)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, storeLocation);
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
            }
            catch (WebException we)
            {
                Console.WriteLine(we.Message);
            }
            catch (NotSupportedException nse)
            {
                Console.WriteLine(nse);
            }
        }
    }
}
