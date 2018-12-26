namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.ApproveSong
{
    using Contracts;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class ApproveSongCommandService : ICommandService<ApproveSong>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public ApproveSongCommandService(
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

        public async Task ExecuteAsync(ApproveSong command)
        {
            Song song = await this.songRepository.GetByIdAsync(command.SongId);

            if (song is null)
            {
                throw new InvalidOperationException(
                    $"Song with id {command.SongId} does not exists!");
            }

            if (song.IsApproved)
            {
                return;
            }

            song.IsApproved = true;
            this.songRepository.Update(song);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
