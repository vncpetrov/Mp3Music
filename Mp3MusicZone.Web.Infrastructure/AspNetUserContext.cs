namespace Mp3MusicZone.Web.Infrastructure
{
    using Domain.Contracts;
    using Domain.Models.Enums;
    using Microsoft.AspNetCore.Http;
    using System;

    public class AspNetUserContext : IUserContext
    {
        private readonly IHttpContextAccessor accessor;

        public AspNetUserContext(IHttpContextAccessor accessor)
        {
            if (accessor is null)
                throw new ArgumentNullException(nameof(accessor));

            this.accessor = accessor;
        }

        public bool IsInRole(Role role)
        {
            return this.accessor.HttpContext.User.IsInRole(role.ToString());
        }
    }
}
