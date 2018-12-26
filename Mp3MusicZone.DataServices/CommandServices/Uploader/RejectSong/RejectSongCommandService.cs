namespace Mp3MusicZone.DomainServices.CommandServices.Uploader.RejectSong
{
    using CommandServices.Songs.DeleteSong;
    using Domain.Contracts;
    using Domain.Models;
    using Mp3MusicZone.DomainServices.Contracts;
    using System;
    using System.Threading.Tasks;

    public class RejectSongCommandService : DeleteSongCommandService, ICommandService<RejectSong>
    {
        public RejectSongCommandService(
            IEfRepository<Song> songRepository,
            ISongProvider songProvider,
            IEfDbContextSaveChanges contextSaveChanges)
            : base(songRepository, songProvider, contextSaveChanges)
        {
        }

        public async Task ExecuteAsync(RejectSong command)
        {
            await base.ExecuteAsync(command);
        }
    }
}
