using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using VisualAcademy.Shared;

namespace UploadApp.Managers
{
    public class FileStorageManager : IFileStorageManager
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageManager(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        public Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, string ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, long ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, int ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(byte[] bytes, string fileName, string folderPath, bool overwrite)
        {
            var path = Path.Combine(_environment.WebRootPath, "files");
            await File.WriteAllBytesAsync(Path.Combine(path, fileName), bytes);
            return fileName;
        }
    }
}
