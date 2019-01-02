namespace Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount
{
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetUsersCountQueryService : IQueryService<GetUsersCount, int>
    {
        private readonly IEfRepository<User> userRepository;

        public GetUsersCountQueryService(IEfRepository<User> userRepository)
        {
            if (userRepository is null)
                throw new ArgumentNullException(nameof(userRepository));

            this.userRepository = userRepository;
        }

        public async Task<int> ExecuteAsync(GetUsersCount query)
            => await this.userRepository.All()
                   .Where(u => u.UserName.ToLower().Contains(
                       query.SearchInfo.SearchTerm.ToLower()))
                   .CountAsync();

    }
}
