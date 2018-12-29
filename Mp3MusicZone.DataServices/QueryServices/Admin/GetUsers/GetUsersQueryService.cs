namespace Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers
{
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
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
        {
            IEnumerable<User> users = await this.userRepository.All(eagerLoading: true)
                        .ToListAsync();

            return users;
        }
    }
}
