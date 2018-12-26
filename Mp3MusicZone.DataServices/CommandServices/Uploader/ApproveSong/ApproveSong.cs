namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.ApproveSong
{
    using Common.Constants;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.ApproveSong)]
    public class ApproveSong
    {
        public string SongId { get; set; }
    }
}
