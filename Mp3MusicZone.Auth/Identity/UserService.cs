namespace Mp3MusicZone.Auth.Identity
{
    using Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;

    public class UserService : UserManager<UserEf>, IUserService
    {
        public UserService(IUserStore<UserEf> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<UserEf> passwordHasher, IEnumerable<IUserValidator<UserEf>> userValidators, IEnumerable<IPasswordValidator<UserEf>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserEf>> logger) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
