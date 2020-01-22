using System;
using System.IO;
using System.Threading.Tasks;
using VisualAcademy.Shared;

namespace UploadApp.Managers
{
    public class FileStorageManager : IFileStorageManager
    {
        public async Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                File.Delete(Path.Combine(folderPath, fileName));
            }
            return await Task.FromResult(true); 
        }

        public async Task<byte[]> DownloadAsync(string fileName, string folderPath)
        {
            if (File.Exists(Path.Combine(folderPath, fileName)))
            {
                byte[] fileBytes = await File.ReadAllBytesAsync(Path.Combine(folderPath, fileName));
                return fileBytes;
            }
            return null; 
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
            await File.WriteAllBytesAsync(Path.Combine(folderPath, fileName), bytes);

            return fileName;
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite)
        {
            using (var fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create))
            {
                await stream.CopyToAsync(fileStream); 
            }

            return fileName;
        }
    }
}
