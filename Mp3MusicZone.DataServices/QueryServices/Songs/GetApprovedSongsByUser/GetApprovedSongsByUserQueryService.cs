namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetApprovedSongsByUser
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Exceptions;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetApprovedSongsByUserQueryService
        : IQueryService<GetApprovedSongsByUser, IEnumerable<Song>>
    {
        private readonly IEfRepository<Song> songRepository;
        private readonly IEfRepository<User> userRepository;

        public GetApprovedSongsByUserQueryService(
            IEfRepository<Song> songRepository,
            IEfRepository<User> userRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            if (userRepository is null)
                throw new ArgumentNullException(nameof(userRepository));

            this.songRepository = songRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<Song>> ExecuteAsync(GetApprovedSongsByUser query)
        {
            User user = await this.userRepository.GetByIdAsync(query.UserId);

            if (user is null)
            {
                throw new NotFoundException(
                   $"User with id {query.UserId} does not exists!");
            }

            IEnumerable<Song> songs = await this.songRepository.All()
                .Where(s => s.UploaderId == query.UserId
                            && s.IsApproved)
                .OrderByDescending(s => s.PublishedOn)
                .ToListAsync();

            return songs;
        }
    }
}
