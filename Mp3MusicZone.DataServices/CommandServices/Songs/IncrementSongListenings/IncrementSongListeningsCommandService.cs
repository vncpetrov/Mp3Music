namespace Mp3MusicZone.DomainServices.CommandServices.Songs.IncrementSongListenings
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class IncrementSongListeningsCommandService : ICommandService<IncrementSongListenings>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges; 

        public IncrementSongListeningsCommandService(
            IEfRepository<Song> songRepository,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.songRepository = songRepository;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task ExecuteAsync(IncrementSongListenings command)
        {
            Song song = await this.songRepository
                .GetByIdAsync(command.SongId);

            if (song is null || !song.IsApproved)
            {
                throw new NotFoundException(
                    $"Song with id {command.SongId} does not exists or is not approved yet!");
            }

            song.Listenings++;
            this.songRepository.Update(song);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
