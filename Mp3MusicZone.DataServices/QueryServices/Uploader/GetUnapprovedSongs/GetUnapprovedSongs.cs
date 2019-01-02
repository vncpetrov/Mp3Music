namespace Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongs
{
    using Common.Constants;
    using Contracts;
    using Domain.Attributes;
    using Domain.Models;
    using System;
    using System.Collections.Generic;

    [Permission(Permissions.GetUnapprovedSongs)]
    public class GetUnapprovedSongs : IQuery<IEnumerable<Song>>
    {
        public int Page { get; set; }
    }
}
