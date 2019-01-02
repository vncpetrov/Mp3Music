namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs
{
    using Contracts;
    using Domain.Models;
    using System;
    using System.Collections.Generic;

    public class GetSongs:IQuery<IEnumerable<Song>>
    {
        public PageInfo PageInfo { get; set; }

        public SearchInfo SearchInfo { get; set; }
    }
}
