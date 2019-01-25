namespace Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying
{
    using Common.Constants;
    using Contracts;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.GetUnapprovedSongForPlaying)]
    public class GetUnapprovedSongForPlaying : IQuery<UnapprovedSongForPlayingDTO>
    {
        public string SongId { get; set; }
    }
}
