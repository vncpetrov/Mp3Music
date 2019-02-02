namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Domain.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class GetSongForDeleteByIdQueryService : IQueryService<GetSongForDeleteById, Song>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetSongForDeleteByIdQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<Song> ExecuteAsync(GetSongForDeleteById query)
        {
            Song song = await this.songRepository.GetByIdAsync(query.SongId);

            if (song is null)
            {
                throw new NotFoundException(
                    $"Song with id {query.SongId} does not exists!");
            }

            return song;
        }
    }
}
