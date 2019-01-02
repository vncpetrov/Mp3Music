namespace Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetUsersQueryService : IQueryService<GetUsers, IEnumerable<User>>
    {
        private readonly IEfRepository<User> userRepository;

        public GetUsersQueryService(IEfRepository<User> userRepository)
        {
            if (userRepository is null)
                throw new ArgumentNullException(nameof(userRepository));

            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> ExecuteAsync(GetUsers query)
            => await this.userRepository.All(eagerLoading: true)
                         .Where(u => u.UserName.ToLower().Contains(
                             query.SearchInfo.SearchTerm.ToLower()))
                        .Skip((query.Page - 1) * 2)
                        .Take(2)
                        .ToListAsync();
    }
}
