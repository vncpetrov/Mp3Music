namespace Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongForPlaying
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class GetUnapprovedSongForPlayingQueryService
        : IQueryService<GetUnapprovedSongForPlaying, UnapprovedSongForPlayingDTO>
    {

        private readonly ISongProvider songProvider;
        private readonly IEfRepository<Song> songRepository;

        public GetUnapprovedSongForPlayingQueryService(
            ISongProvider songProvider,
            IEfRepository<Song> songRepository)
        {
            if (songProvider is null)
                throw new ArgumentNullException(nameof(songProvider));

            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songProvider = songProvider;
            this.songRepository = songRepository;
        }

        public async Task<UnapprovedSongForPlayingDTO> ExecuteAsync(
            GetUnapprovedSongForPlaying query)
        {
            Song song = await this.songRepository.GetByIdAsync(query.SongId);

            if (song is null)
            {
                throw new InvalidOperationException(
                    $"Song with id {query.SongId} does not exists!");
            }

            byte[] songFile = await this.songProvider
                 .GetAsync(song.Title, song.FileExtension);

            return new UnapprovedSongForPlayingDTO()
            {
                FileExtension = song.FileExtension,
                File = songFile
            };
        }
    }
}
