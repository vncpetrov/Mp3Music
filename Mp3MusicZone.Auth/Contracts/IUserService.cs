namespace Mp3MusicZone.Auth.Contracts
{
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public interface IUserService
    {
        IdentityOptions Options { get; }

        string GetUserId(ClaimsPrincipal principal);

        Task<IdentityResult> CreateAsync(UserEf user);
        Task<IdentityResult> CreateAsync(UserEf user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(UserEf user);

        Task<IdentityResult> AddLoginAsync(UserEf user, UserLoginInfo login);

        Task<UserEf> FindByIdAsync(string userId);

        Task<IdentityResult> ConfirmEmailAsync(UserEf user, string token);

        Task<UserEf> FindByEmailAsync(string email);

        Task<bool> IsEmailConfirmedAsync(UserEf user);

        Task<string> GeneratePasswordResetTokenAsync(UserEf user);

        Task<IdentityResult> ResetPasswordAsync(UserEf user, string token, string newPassword);

        Task<UserEf> GetUserAsync(ClaimsPrincipal principal);

        Task<IdentityResult> SetEmailAsync(UserEf user, string email);

        Task<bool> HasPasswordAsync(UserEf user);

        Task<IdentityResult> ChangePasswordAsync(UserEf user, string currentPassword, string newPassword);

        Task<IdentityResult> AddPasswordAsync(UserEf user, string password);

        Task<IList<UserLoginInfo>> GetLoginsAsync(UserEf user);

        Task<IdentityResult> RemoveLoginAsync(UserEf user, string loginProvider,
            string providerKey);

        Task<string> GetAuthenticatorKeyAsync(UserEf user);

        Task<int> CountRecoveryCodesAsync(UserEf user);

        Task<IdentityResult> SetTwoFactorEnabledAsync(UserEf user, bool enabled);

        Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(UserEf user,
            int number);

        Task<IdentityResult> ResetAuthenticatorKeyAsync(UserEf user);

        Task<bool> VerifyTwoFactorTokenAsync(UserEf user, string tokenProvider, string token);

        string GetUserName(ClaimsPrincipal principal);

        Task<UserEf> FindByNameAsync(string userName);

        Task<IdentityResult> AddToRoleAsync(UserEf user, string role);

        Task<IdentityResult> UpdateAsync(UserEf user);

        bool CheckPermission(string permissionId);
    }
}