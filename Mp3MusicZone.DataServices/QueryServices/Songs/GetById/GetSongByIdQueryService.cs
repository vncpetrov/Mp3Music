namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetById
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class GetSongByIdQueryService : IQueryService<GetSongById, Song>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetSongByIdQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<Song> ExecuteAsync(GetSongById query)
        {
            Song song = await this.songRepository.GetByIdAsync(query.SongId);

            if (song is null)
            {
                throw new InvalidOperationException($"Song with id {query.SongId} does not exists!");
            }

            return song;
        }
    }
}
