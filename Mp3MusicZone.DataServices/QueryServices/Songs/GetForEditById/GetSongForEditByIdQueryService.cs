namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetForEditById
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class GetSongForEditByIdQueryService : IQueryService<GetSongForEditById, Song>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetSongForEditByIdQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<Song> ExecuteAsync(GetSongForEditById query)
        {
            Song song = await this.songRepository.GetByIdAsync(query.SongId);

            if (song is null)
            {
                throw new InvalidOperationException(
                    $"Song with id {query.SongId} does not exists!");
            }

            return song;
        }
    }
}
