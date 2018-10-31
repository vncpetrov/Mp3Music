namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetLastApproved
{
    using Contracts;
    using Domain.Models;
    using System;
    using System.Collections.Generic;

    public class GetLastApprovedSongs:IQuery<IEnumerable<Song>>
    {
        public int Count { get; set; }
    }
}
