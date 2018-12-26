namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.RejectSong
{
    using CommandServices.Songs.DeleteSong;
    using Common.Constants;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.RejectSong)]
    public class RejectSong : DeleteSong
    {
    }
}
