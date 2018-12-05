namespace Mp3MusicZone.Auth.Identity
{
    using Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Collections.Generic;

    public class UserService : UserManager<UserEf>, IUserService
    {
        private IUserPermissionChecker permissionChecker;

        public UserService(
            IUserStore<UserEf> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserEf> passwordHasher,
            IEnumerable<IUserValidator<UserEf>> userValidators, 
            IEnumerable<IPasswordValidator<UserEf>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserEf>> logger,
            IUserPermissionChecker permissionChecker) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            if (permissionChecker is null)
                throw new ArgumentNullException(nameof(permissionChecker));

            this.permissionChecker = permissionChecker;
        }

        public bool CheckPermission(string permissionId)
        {
            try
            {
                this.permissionChecker.CheckPermission(permissionId);
            }
            catch (NotAuthorizedException)
            {
                return false;
            }

            return true;
        }
    }
}
