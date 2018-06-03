namespace Mp3MusicZone.Auth.Contracts
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Threading.Tasks;

    public interface IRoleService
    {
        Task<bool> RoleExistsAsync(string roleName);

        Task<IdentityResult> CreateAsync(string roleName);
    }
}
