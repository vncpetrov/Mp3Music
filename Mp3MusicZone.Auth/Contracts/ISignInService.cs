namespace Mp3MusicZone.Auth.Contracts
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface ISignInService
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password,
            bool isPersistent, bool lockoutOnFailure);

        Task<EfDataAccess.Models.UserEf> GetTwoFactorAuthenticationUserAsync();

        Task<SignInResult> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent,
            bool rememberClient);

        Task<SignInResult> TwoFactorRecoveryCodeSignInAsync(string recoveryCode);

        Task SignInAsync(EfDataAccess.Models.UserEf user, bool isPersistent, string authenticationMethod = null);

        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl, string userId = null);

        Task SignOutAsync();

        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null);

        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey,
            bool isPersistent, bool bypassTwoFactor);

        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();

        bool IsSignedIn(ClaimsPrincipal principal); 
    }
}
