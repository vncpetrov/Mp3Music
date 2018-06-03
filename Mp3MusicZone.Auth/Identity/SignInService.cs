namespace Mp3MusicZone.Auth.Identity
{
    using Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;

    public class SignInService : SignInManager<UserEf>, ISignInService
    {
        public SignInService(UserManager<UserEf> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<UserEf> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<UserEf>> logger, IAuthenticationSchemeProvider schemes) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }
    }
}
