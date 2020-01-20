using Microsoft.AspNetCore.Components;
using UploadApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadApp.Pages.Uploads
{
    public partial class Index
    {
        [Inject]
        public IUploadRepository UploadRepositoryAsyncReference { get; set; }

        [Inject]
        public NavigationManager NavigationManagerReference { get; set; }

        protected List<Upload> models;

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
            //await Task.Delay(3000);
            var resultsSet = await UploadRepositoryAsyncReference.GetAllAsync(pager.PageIndex, pager.PageSize);
            pager.RecordCount = resultsSet.TotalRecords;
            models = resultsSet.Records.ToList();
        }

        private async Task SearchData()
        {
            var resultsSet = await UploadRepositoryAsyncReference.SearchAllAsync(pager.PageIndex, pager.PageSize, this.searchQuery);
            pager.RecordCount = resultsSet.TotalRecords;
            models = resultsSet.Records.ToList();
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

        private string searchQuery = "";

        protected async void Search(string query)
        {
            this.searchQuery = query;

            await SearchData();

            StateHasChanged();
        }
    }
}
