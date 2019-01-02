﻿
namespace Mp3MusicZone.DomainServices.QueryServices
{
    using System;

    public class PageInfo
    {
        public PageInfo(int page, int pageSize)
        {
            this.Page = page;
            this.PageSize = pageSize;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
