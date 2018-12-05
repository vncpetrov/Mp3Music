namespace Mp3MusicZone.Auth.Identity
{
    using Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Mp3MusicZone.EfDataAccess;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoleService : RoleManager<RoleEf>, IRoleService
    {

        public RoleService(IRoleStore<RoleEf> store, IEnumerable<IRoleValidator<RoleEf>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<RoleEf>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }


        public Task<IdentityResult> CreateAsync(string roleName)
        {
            return base.CreateAsync(
                new RoleEf()
                {
                    Name = roleName
                });
        }
    }
}
