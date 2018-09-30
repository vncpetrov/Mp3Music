namespace Mp3MusicZone.DataServices
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using System;
    using System.Threading.Tasks;

    public class SongService : ISongService
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly ISongProvider songProvider;
        private readonly IDateTimeProvider timeProvider;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public SongService(
            IEfRepository<Song> songRepository,
            ISongProvider songProvider,
            IDateTimeProvider timeProvider,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            if (songProvider is null)
                throw new ArgumentNullException(nameof(songProvider));

            if (timeProvider is null)
                throw new ArgumentNullException(nameof(timeProvider));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.songRepository = songRepository;
            this.songProvider = songProvider;
            this.timeProvider = timeProvider;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task UploadAsync(
            string title,
            string extension,
            string singer, 
            int releasedYear,
            string uploaderId, 
            byte[] file)
        {
            Song song = new Song()
            {
                Title = title.Trim(),
                Singer = singer.Trim(),
                ReleasedYear = releasedYear,
                UploaderId = uploaderId,
                PublishedOn = this.timeProvider.UtcNow
            };

            this.songRepository.Add(song);

            await this.songProvider.WriteAsync(
                title,
                extension,
                file);

            this.contextSaveChanges.SaveChanges();
        }
    }
}
