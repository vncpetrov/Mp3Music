namespace Mp3MusicZone.DomainServices.CommandServices.Songs.DeleteSong
{
    using Common.Constants;
    using Domain.Attributes;
    using QueryServices.Songs.GetLastApproved;
    using System;
    
    [Permission(Permissions.DeleteSong)]
    [InvalidateCacheFor(nameof(GetLastApprovedSongs))] 
    public class DeleteSong
    {
        public string SongId { get; set; }
    }
}
