using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using UploadApp.Models;

namespace UploadApp.Controllers
{
    public class UploadDownloadController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUploadRepository _repository;

        public UploadDownloadController(IWebHostEnvironment environment, IUploadRepository repository)
        {
            this._environment = environment;
            this._repository = repository;
        }

        /// <summary>
        /// 게시판 파일 강제 다운로드 기능(/BoardDown/:Id)
        /// </summary>
        public async Task<FileResult> BoardDown(int id)
        {
            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return null;
            }
            else
            {

                if (System.IO.File.Exists(Path.Combine(_environment.WebRootPath, "files") + "\\" + model.FileName))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, "files") + "\\" + model.FileName);

                    return File(fileBytes, "application/octet-stream", model.FileName);
                }

                return null;
            }
        }
    }
}
