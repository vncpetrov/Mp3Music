namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetForEditById
{
    using Common.Constants;
    using Contracts;
    using Domain.Attributes;
    using Domain.Models;
    using System;

    [Permission(Permissions.EditSong)]
    public class GetSongForEditById : IQuery<Song>
    {
        public string SongId { get; set; }
    }
}
