namespace Mp3MusicZone.DataServices.SongServices
{
    using System;

    public class EditSong
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public string Singer { get; set; }

        public int ReleasedYear { get; set; }

        public string FileExtension { get; set; }

        public byte[] SongFile { get; set; }
    }
}
