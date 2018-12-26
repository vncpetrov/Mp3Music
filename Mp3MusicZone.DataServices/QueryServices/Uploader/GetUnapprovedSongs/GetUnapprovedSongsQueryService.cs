namespace Mp3MusicZone.DomainServices.QueryServices.Uploader.GetUnapprovedSongs
{
    using Common.Constants;
    using Contracts;
    using Domain.Attributes;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetUnapprovedSongsQueryService
        : IQueryService<GetUnapprovedSongs, IEnumerable<Song>>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetUnapprovedSongsQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<IEnumerable<Song>> ExecuteAsync(GetUnapprovedSongs query)
             => await this.songRepository.All()
                   .Where(s => s.IsApproved == false)
                   .ToListAsync();

    }
}
