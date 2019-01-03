namespace Mp3MusicZone.Web.ViewModels
{
    using Shared;
    using System;
    using System.Collections.Generic;

    public static class ViewModelFactory
    {
        public static SearchViewModel<PaginatedViewModel<TModel>> CreateSearchPaginatedViewModel<TModel>(
            IEnumerable<TModel> items,
            int currentPage,
            int pageSize,
            int itemsCount,
            string searchTerm,
            string searchIn)
            where TModel : class
            => new SearchViewModel<PaginatedViewModel<TModel>>(
                new PaginatedViewModel<TModel>(
                    items,
                    currentPage,
                    pageSize,
                    itemsCount),
                searchTerm,
                searchIn);

            public static PaginatedViewModel<TModel> CreatePaginatedViewModel<TModel>(
            IEnumerable<TModel> items,
            int currentPage,
            int pageSize,
            int itemsCount)
            where TModel : class
            => new PaginatedViewModel<TModel>(
                    items,
                    currentPage,
                    pageSize,
                    itemsCount);
    }
}
