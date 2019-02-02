namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetApprovedSongsByUser
{
    using Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Collections.Generic;

    public class GetApprovedSongsByUser : IQuery<IEnumerable<Song>>
    {
        public string UserId { get; set; }
    }
}
