namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.RejectSong
{
    using Common.Constants;
    using Domain.Attributes;
    using Songs.DeleteSong;
    using System;

    [Permission(Permissions.RejectSong)]
    public class RejectSong : DeleteSong
    {
    }
}
