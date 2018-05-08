namespace Mp3MusicZone.Auth.Identity
{
    using Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RoleService : RoleManager<IdentityRole>, IRoleService
    {
        public RoleService(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        public Task<IdentityResult> CreateAsync(string roleName)
        {
            return base.CreateAsync(new IdentityRole(roleName));
        }
    }
}
