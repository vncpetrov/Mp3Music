namespace Mp3MusicZone.Web.FacadeServices
{
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
    using System;
    using System.Threading.Tasks;

    // Does it sounds like a Domain concept?
    public interface ISongPlayer
    {
        Task<SongForPlayingDTO> GetSongAsync(string songId);
    }
}
