namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying
{
    using System;

    public class SongForPlayingDTO
    {
        public string FileExtension { get; set; }

        public string HeadingText { get; set; }

        public byte[] File { get; set; }
    }
}
