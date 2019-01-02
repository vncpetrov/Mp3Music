namespace Mp3MusicZone.DomainServices.QueryServices.Songs.GetSongsCount
{
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetSongsCountQueryService : IQueryService<GetSongsCount, int>
    {
        private readonly IEfRepository<Song> songRepository;

        public GetSongsCountQueryService(IEfRepository<Song> songRepository)
        {
            if (songRepository is null)
                throw new ArgumentNullException(nameof(songRepository));

            this.songRepository = songRepository;
        }

        public async Task<int> ExecuteAsync(GetSongsCount query)
            => await this.songRepository.All()
                   .Where(s => s.IsApproved == query.Approved 
                               && s.Title.ToLower().Contains(
                                   query.SearchInfo.SearchTerm.ToLower()))
                   .CountAsync();
    }
}
