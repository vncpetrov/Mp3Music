namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.ApproveSong
{
    using Common.Constants;
    using Domain.Attributes;
    using QueryServices.Songs.GetLastApproved;
    using System;

    [Permission(Permissions.ApproveSong)]
    [InvalidateCacheFor(nameof(GetLastApprovedSongs))]
    public class ApproveSong
    {
        public string SongId { get; set; }
    }
}
