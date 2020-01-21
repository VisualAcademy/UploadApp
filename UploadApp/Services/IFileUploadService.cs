using BlazorInputFile;
using System.Threading.Tasks;

namespace UploadApp.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(IFileListEntry file);
    }
}
