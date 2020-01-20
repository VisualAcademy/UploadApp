using Microsoft.AspNetCore.Components;
using UploadApp.Models;

namespace UploadApp.Pages.Uploads
{
    public partial class Create
    {
        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected Upload model = new Upload();

        public string ParentId { get; set; }

        protected int[] parentIds = { 1, 2, 3 };

        protected async void FormSubmit()
        {
            int.TryParse(ParentId, out int parentId);
            model.ParentId = parentId; 
            await UploadRepositoryAsyncReference.AddAsync(model);
            NavigationManagerReference.NavigateTo("/Uploads");
        }
    }
}
