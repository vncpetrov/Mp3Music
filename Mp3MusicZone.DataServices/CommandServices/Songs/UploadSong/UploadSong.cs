namespace Mp3MusicZone.DomainServices.CommandServices.Songs.UploadSong
{
    using Common.Constants;
    using Domain.Attributes;
    using System;

    [Permission(Permissions.UploadSong)]
    public class UploadSong
    {        
        public string Title { get; set; }

        public string Singer { get; set; }

        public int ReleasedYear { get; set; }

        public string FileExtension { get; set; }

        public byte[] SongFile { get; set; }

        public string UploaderId { get; set; }
    }
}
