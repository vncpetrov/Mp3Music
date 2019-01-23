namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongs
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetSongsQueryService : IQueryService<GetSongs, IEnumerable<Song>>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetSongsQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<IEnumerable<Song>> ExecuteAsync(GetSongs query)
            => await this.songRepository.All()
                         .Where(s => s.IsApproved == true
                                     && s.Title.ToLower().Contains(
                                         query.SearchInfo.SearchTerm.ToLower()))
                         .OrderByDescending(s => s.Id)
                         .Skip((query.PageInfo.Page - 1) * query.PageInfo.PageSize)
                         .Take(query.PageInfo.PageSize)
                         .ToListAsync();
    }
}
