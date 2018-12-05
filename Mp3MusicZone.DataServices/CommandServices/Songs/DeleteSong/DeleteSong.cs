namespace Mp3MusicZone.DomainServices.CommandServices.Songs.DeleteSong
{
    using Common.Constants;
    using Domain.Attributes;
    using System;
    
    [Permission(Permissions.DeleteSong)]
    public class DeleteSong
    {
        public string SongId { get; set; }
    }
}
