namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying
{
    using Contracts;
    using System;

    public class GetSongForPlaying : IQuery<SongForPlayingDTO>
    {
        public string SongId { get; set; }
    }
}
