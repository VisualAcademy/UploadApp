using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using UploadApp.Models;
using VisualAcademy.Shared;

namespace UploadApp.Controllers
{
    public class UploadDownloadController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUploadRepository _repository;
        private readonly IFileStorageManager _fileStorageManager;

        public UploadDownloadController(IWebHostEnvironment environment, IUploadRepository repository, IFileStorageManager fileStorageManager)
        {
            this._environment = environment;
            this._repository = repository;
            this._fileStorageManager = fileStorageManager;
        }

        /// <summary>
        /// 게시판 파일 강제 다운로드 기능(/BoardDown/:Id)
        /// </summary>
        public async Task<IActionResult> FileDown(int id)
        {
            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return null;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.FileName))
                {
                    byte[] fileBytes = await _fileStorageManager.DownloadAsync(model.FileName, "");
                    if (fileBytes != null)
                    {
                        // DownCount
                        model.DownCount = model.DownCount + 1;
                        await _repository.EditAsync(model); 

                        return File(fileBytes, "application/octet-stream", model.FileName);
                    }
                    else
                    {
                        return Redirect("/");
                    }
                }

                return Redirect("/");
            }
        }
    }
}
