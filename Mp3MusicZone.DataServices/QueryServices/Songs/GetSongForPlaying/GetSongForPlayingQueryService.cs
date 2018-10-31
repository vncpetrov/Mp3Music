namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongForPlaying
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class GetSongForPlayingQueryService 
        : IQueryService<GetSongForPlaying, SongForPlayingDTO>
    {
        private readonly ISongProvider songProvider;
        private readonly IEfRepository<Song> songRepository;

        public GetSongForPlayingQueryService(
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

        public async Task<SongForPlayingDTO> ExecuteAsync(GetSongForPlaying query)
        {
            Song song = await this.songRepository.GetByIdAsync(query.SongId);

            if (song is null)
            {
                throw new InvalidOperationException($"Song with id {query.SongId} does not exists!");
            }

           byte[] songFile = await this.songProvider
                .GetAsync(song.Title, song.FileExtension);

            return new SongForPlayingDTO()
            {
                FileExtension = song.FileExtension,
                HeadingText = $"{song.Singer} - {song.Title}, {song.ReleasedYear}.{song.FileExtension}",
                File = songFile 
            };
        }

    }
}
