namespace Mp3MusicZone.Web.Infrastructure
{
    using Domain.Contracts;
    using Domain.Models.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Security.Claims;

    public class AspNetUserContext : IUserContext
    {
        private readonly IHttpContextAccessor accessor = new HttpContextAccessor();

        public AspNetUserContext()
        {
        }

        public bool IsInRole(RoleType role)
            => this.accessor.HttpContext
                .User
                .IsInRole(role.ToString());

        public string GetCurrentUserId()
            => this.accessor.HttpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        
    }
}
