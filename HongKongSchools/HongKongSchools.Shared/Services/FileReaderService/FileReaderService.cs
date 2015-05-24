using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HongKongSchools.Services.FileReaderService
{
    public class FileReaderService : IFileReaderService
    {
        public const string DATA_FOLDER_PATH = "Data";

        public async Task<string> ReadFile(IStorageFolder folder, string fileName)
        {
            var dataFolder = await folder.GetFolderAsync(DATA_FOLDER_PATH);
            var file = await dataFolder.GetFileAsync(fileName);
            var content = await FileIO.ReadTextAsync(file);

            return content;
        }
    }
}
