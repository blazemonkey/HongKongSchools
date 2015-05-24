using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HongKongSchools.Services.FileReaderService
{
    public interface IFileReaderService
    {
        Task<string> ReadFile(IStorageFolder folder, string fileName);
    }
}
