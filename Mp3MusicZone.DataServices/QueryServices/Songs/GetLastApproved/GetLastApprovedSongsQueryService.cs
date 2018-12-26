namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetLastApproved
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetLastApprovedSongsQueryService
        : IQueryService<GetLastApprovedSongs, IEnumerable<Song>>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetLastApprovedSongsQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<IEnumerable<Song>> ExecuteAsync(GetLastApprovedSongs query)
             => await this.songRepository.All()
                    .Where(s => s.IsApproved == true)
                    .OrderByDescending(s => s.Id)
                    .Take(query.Count)
                    .ToListAsync();
    }
}
