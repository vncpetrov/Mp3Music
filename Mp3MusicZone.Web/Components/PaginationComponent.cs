namespace Mp3MusicZone.Web.Components
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System;
    using ViewModels.Shared;

    public class PaginationComponent : ViewComponent
    {
        public IViewComponentResult Invoke(
            IPagination pageInfo,
            string searchTerm,
            string actionToCall)
        {
            int totalPages =
                (int)Math.Ceiling((double)pageInfo.TotalItems / pageInfo.PageSize);

            if (pageInfo.Current < 1)
            {
                pageInfo.Current = 1;
            }

            if (totalPages > 0 && pageInfo.Current > totalPages)
            {
                pageInfo.Current = totalPages;
            }

            int startPage = pageInfo.Current - 1 <= 0
                ? 1
                : pageInfo.Current - 1;

            int endPage = pageInfo.Current + 1 >= totalPages
                ? totalPages
                : pageInfo.Current + 1;

            if (startPage == 1)
            {
                endPage = totalPages >= 3 ? 3 : totalPages;
            }

            if (endPage == totalPages && pageInfo.Current == endPage)
            {
                startPage = startPage == 1 ? 1 : startPage - 1;
            }

            string previousDisabled =
                pageInfo.Current <= 1 ? "disabled" : string.Empty;
            string nextDisabled =
                pageInfo.Current >= totalPages ? "disabled" : string.Empty;

            PaginationComponentViewModel model = new PaginationComponentViewModel()
            {
                SearchTerm = searchTerm,
                Current = pageInfo.Current,
                StartPage = startPage,
                EndPage = endPage,
                PreviousDisabled = previousDisabled,
                NextDisabled = nextDisabled,
                ActionToCall = actionToCall
            };

            return View("_Pagination", model);
        }
    }
}
