namespace Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying
{
    using System;

    public class UnapprovedSongForPlayingDTO
    {
        public string FileExtension { get; set; }

        public byte[] File { get; set; }
    }
}
