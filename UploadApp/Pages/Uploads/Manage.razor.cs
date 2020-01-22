using Microsoft.AspNetCore.Components;
using UploadApp.Models;
using UploadApp.Pages.Uploads.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisualAcademy.Shared;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.JSInterop;
using BlazorUtils;

namespace UploadApp.Pages.Uploads
{
    public partial class Manage
    {
        [Parameter]
        public int ParentId { get; set; } = 0;

        [Parameter]
        public string ParentKey { get; set; } = "";

        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        /// <summary>
        /// EditorForm에 대한 참조: 모달로 글쓰기 또는 수정하기
        /// </summary>
        public EditorForm EditorFormReference { get; set; }

        /// <summary>
        /// DeleteDialog에 대한 참조: 모달로 항목 삭제하기 
        /// </summary>
        public DeleteDialog DeleteDialogReference { get; set; }
        
        protected List<Upload> models;

        protected Upload model = new Upload();

        /// <summary>
        /// 공지사항으로 올리기 폼을 띄울건지 여부 
        /// </summary>
        public bool IsInlineDialogShow { get; set; } = false; 

        protected DulPager.DulPagerBase pager = new DulPager.DulPagerBase()
        {
            PageNumber = 1,
            PageIndex = 0,
            PageSize = 2,
            PagerButtonCount = 5
        };

        protected override async Task OnInitializedAsync()
        {
            if (this.searchQuery != "")
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }
        }

        private async Task DisplayData()
        {
            if (ParentKey == "")
            {
                var resultsSet = await UploadRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
                pager.RecordCount = resultsSet.TotalRecords;
                models = resultsSet.Records.ToList();
            }
            else
            {
                var resultsSet = await UploadRepositoryAsyncReference.GetAllByParentKeyAsync(pager.PageIndex, pager.PageSize, ParentKey);
                pager.RecordCount = resultsSet.TotalRecords;
                models = resultsSet.Records.ToList();
            }

            StateHasChanged();
        }

        private async Task SearchData()
        {
            if (ParentKey == "")
            {
                var resultsSet = await UploadRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
                pager.RecordCount = resultsSet.TotalRecords;
                models = resultsSet.Records.ToList();
            }
            else
            {
                var resultsSet = await UploadRepositoryAsyncReference.SearchAllByParentKeyAsync(pager.PageIndex, pager.PageSize, this.searchQuery, ParentKey);
                pager.RecordCount = resultsSet.TotalRecords;
                models = resultsSet.Records.ToList();
            }
        }

        protected void NameClick(int id)
        {
            NavigationManagerReference.NavigateTo($"/Uploads/Details/{id}");
        }

        protected async void PageIndexChanged(int pageIndex)
        {
            pager.PageIndex = pageIndex;
            pager.PageNumber = pageIndex + 1;

            if (this.searchQuery == "")
            {
                await DisplayData();
            }
            else
            {
                await SearchData();
            }

            StateHasChanged();
        }

        public string EditorFormTitle { get; set; } = "CREATE";

        protected void ShowEditorForm()
        {
            EditorFormTitle = "CREATE";
            this.model = new Upload();
            this.model.ParentKey = ParentKey; // 
            EditorFormReference.Show();
        }

        protected void EditBy(Upload model)
        {
            EditorFormTitle = "EDIT";
            this.model = new Upload();
            this.model = model;
            this.model.ParentKey = ParentKey; // 
            EditorFormReference.Show();
        }

        protected void DeleteBy(Upload model)
        {
            this.model = model;
            DeleteDialogReference.Show();
        }

        protected void ToggleBy(Upload model)
        {
            this.model = model;
            IsInlineDialogShow = true; 
        }

        protected async void DownloadBy(Upload model)
        {
            if (!string.IsNullOrEmpty(model.FileName))
            {
                byte[] fileBytes = await FileStorageManager.DownloadAsync(model.FileName, "");
                if (fileBytes != null)
                {
                    // DownCount
                    model.DownCount = model.DownCount + 1;
                    await UploadRepositoryAsyncReference.EditAsync(model);

                    await FileUtil.SaveAs(JSRuntime, model.FileName, fileBytes); 
                }
            }
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IFileStorageManager FileStorageManager { get; set; }

        //[Inject]
        //public IWebHostEnvironment WebHostEnvironment { get; set; }

        protected async void CreateOrEdit()
        {
            EditorFormReference.Hide();
            this.model = new Upload();
            await DisplayData();            
        }

        protected async void DeleteClick()
        {
            // 첨부 파일 삭제 
            await FileStorageManager.DeleteAsync(model.FileName, "");

            await UploadRepositoryAsyncReference.DeleteAsync(this.model.Id);
            DeleteDialogReference.Hide();
            this.model = new Upload(); 
            await DisplayData();
        }

        protected void ToggleClose()
        {
            IsInlineDialogShow = false;
            this.model = new Upload(); 
        }

        protected async void ToggleClick()
        {
            this.model.IsPinned = (this.model?.IsPinned == true) ? false : true; 

            await UploadRepositoryAsyncReference.EditAsync(this.model);
            IsInlineDialogShow = false; 
            this.model = new Upload();
            await DisplayData();
        }

        private string searchQuery = "";

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }
    }
}
