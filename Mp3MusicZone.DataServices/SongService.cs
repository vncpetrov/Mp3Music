namespace Mp3MusicZone.DomainServices
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            if (this.songRepository.All
                        .Any(s => s.Title.ToLower() == title.ToLower()))
            {
                throw new InvalidOperationException($"Song {title} already exists!");
            }

            Song song = new Song()
            {
                Title = title.Trim(),
                Singer = singer.Trim(),
                ReleasedYear = releasedYear,
                UploaderId = uploaderId,
                PublishedOn = this.timeProvider.UtcNow,
                FileExtension = extension
            };

            this.songRepository.Add(song);

            await this.songProvider.WriteAsync(
                title,
                extension,
                file);

            this.contextSaveChanges.SaveChanges();
        }

        public async Task EditAsync(
            int songId,
            string title,
            string extension,
            string singer,
            int releasedYear,
            byte[] file)
        {
            Song song = await this.songRepository.GetByIdAsync(songId);
            extension = extension is null ? song.FileExtension : extension;

            if (file is null && song.Title != title)
            {
                this.songProvider.Rename(song.Title, title, extension);
            }
            else if (file != null)
            {
                this.songProvider.Delete(song.Title, extension);
                await this.songProvider.WriteAsync(title, extension, file);
            }

            song.Title = title;
            song.Singer = singer;
            song.ReleasedYear = releasedYear;
            song.FileExtension = extension;

            this.songRepository.Update(song);
            this.contextSaveChanges.SaveChanges();
        }

        public async Task<IEnumerable<Song>> GetLastApprovedAsync(int count)
            => await this.songRepository.All
                                        .OrderByDescending(s => s.Id)
                                        .Take(count)
                                        .ToListAsync();

        public async Task<Song> GetByIdAsync(int songId)
        {
            Song song = await this.songRepository.GetByIdAsync(songId);

            if (song is null)
            {
                throw new InvalidOperationException($"Song with id {songId} does not exists!");
            }

            return song;
        }

        public async Task<byte[]> GetSongFileAsync(Song song)
            => await this.songProvider
                         .GetAsync(song.Title, song.FileExtension);
    }
}
