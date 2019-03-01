namespace Mp3MusicZone.DomainServices.CommandServices.Songs.EditSong
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class EditSongCommandService : ICommandService<EditSong>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly ISongProvider songProvider;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public EditSongCommandService(
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

        public async Task ExecuteAsync(EditSong command)
        {
            Song song = await this.songRepository
                .GetByIdAsync(command.SongId);

            if (song is null)
            {
                throw new NotFoundException(
                    $"Song with id {command.SongId} does not exists!");
            }

            command.FileExtension = command.FileExtension is null ?
                song.FileExtension :
                command.FileExtension;

            if (command.SongFile is null && song.Title != command.Title)
            {
                this.songProvider.Update(song.Title, command.Title, command.FileExtension);
            }
            else if (command.SongFile != null)
            {
                this.songProvider.Delete(song.Title, command.FileExtension);
                await this.songProvider.AddAsync(command.Title, command.FileExtension, command.SongFile);
            }

            song.Title = command.Title;
            song.Singer = command.Singer;
            song.ReleasedYear = command.ReleasedYear;
            song.FileExtension = command.FileExtension;

            this.songRepository.Update(song);
            this.contextSaveChanges.SaveChanges();
        }

    }
}
