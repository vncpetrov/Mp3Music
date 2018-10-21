﻿namespace Mp3MusicZone.DataServices.SongServices
{
    using Contracts;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UploadSongService : ICommandService<UploadSong>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly ISongProvider songProvider;
        private readonly IDateTimeProvider timeProvider;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public UploadSongService(
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

        public async Task ExecuteAsync(UploadSong command)
        {
            if (this.songRepository.All
                        .Any(s => s.Title.ToLower() == command.Title.ToLower()))
            {
                throw new InvalidOperationException($"Song {command.Title} already exists!");
            }

            Song song = new Song()
            {
                Title = command.Title.Trim(),
                Singer = command.Singer.Trim(),
                ReleasedYear = command.ReleasedYear,
                UploaderId = command.UploaderId,
                PublishedOn = this.timeProvider.UtcNow,
                FileExtension = command.FileExtension
            };

            this.songRepository.Add(song);

            await this.songProvider.WriteAsync(
                command.Title,
                command.FileExtension,
                command.SongFile);

            this.contextSaveChanges.SaveChanges();
        }
    }
}
