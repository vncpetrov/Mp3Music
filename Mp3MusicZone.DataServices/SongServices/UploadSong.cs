namespace Mp3MusicZone.DataServices.SongServices
{
    using System;

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
