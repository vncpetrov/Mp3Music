namespace Mp3MusicZone.DomainServices.CommandServices.Songs.DeleteSong
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Exceptions;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class DeleteSongCommandService : ICommandService<DeleteSong>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly ISongProvider songProvider;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public DeleteSongCommandService(
            IEfRepository<Song> songRepository,
            ISongProvider songProvider,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            if (songProvider is null)
                throw new ArgumentNullException(nameof(songProvider));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.songRepository = songRepository;
            this.songProvider = songProvider;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task ExecuteAsync(DeleteSong command)
        {
            Song song = await this.songRepository
                .GetByIdAsync(command.SongId);

            if (song is null)
            {
                throw new NotFoundException(
                    $"Song with id {command.SongId} does not exists!");
            }

            this.songProvider.Delete(song.Title, song.FileExtension);
            this.songRepository.Delete(song);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
