namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById
{
    using Common.Constants;
    using Contracts;
    using Domain.Attributes;
    using Domain.Models;
    using System;

    [Permission(Permissions.DeleteSong)]
    public class GetSongForDeleteById : IQuery<Song>
    {
        public string SongId { get; set; }
    }
}
