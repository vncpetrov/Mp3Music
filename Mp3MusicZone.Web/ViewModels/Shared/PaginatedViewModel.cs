namespace Mp3MusicZone.Web.ViewModels.Shared
{
    using System;
    using System.Collections.Generic;

    public class PaginatedViewModel<TModel> : IPagination
        where TModel : class
    {
        public PaginatedViewModel(
            IEnumerable<TModel> items,
            int current,
            int pageSize,
            int totalItems)
        {
            this.Items = items;
            this.Current = current;
            this.PageSize = pageSize;
            this.TotalItems = totalItems;
        }

        public IEnumerable<TModel> Items { get; set; }

        public int Current { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }
    }
}
