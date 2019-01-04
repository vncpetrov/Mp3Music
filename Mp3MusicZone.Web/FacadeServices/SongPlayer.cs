namespace Mp3MusicZone.Web.FacadeServices
{
    using DomainServices.CommandServices.Songs.IncrementSongListenings;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
    using System;
    using System.Threading.Tasks;

    public class SongPlayer : ISongPlayer
    {
        private readonly ICommandService<IncrementSongListenings> incrementSongListenings;
        private readonly IQueryService<GetSongForPlaying, SongForPlayingDTO> getSong;

        public SongPlayer(
            ICommandService<IncrementSongListenings> incrementSongListenings,
            IQueryService<GetSongForPlaying, SongForPlayingDTO> getSong)
        {
            if (incrementSongListenings is null)
                throw new ArgumentNullException(nameof(incrementSongListenings));

            if (getSong is null)
                throw new ArgumentNullException(nameof(getSong));

            this.incrementSongListenings = incrementSongListenings;
            this.getSong = getSong;
        }

        public async Task<SongForPlayingDTO> GetSongAsync(string songId)
        {
            IncrementSongListenings command = new IncrementSongListenings()
            {
                SongId = songId
            };

            await this.incrementSongListenings.ExecuteAsync(command);

            GetSongForPlaying query = new GetSongForPlaying()
            {
                SongId = songId
            };

            SongForPlayingDTO song = await this.getSong.ExecuteAsync(query);

            return song;            
        }
    }
}
