namespace Mp3MusicZone.Web.Infrastructure.AspNetOnStartupCommandServices.RegisterRoles
{
    using Auth.Contracts;
    using Domain.Models.Enums;
    using DomainServices.CommandServices.OnStartup;
    using DomainServices.Contracts;
    using System;
    using System.Threading.Tasks;

    public class RegisterRolesCommandService : ICommandService<OnStartupNullObject>
    {
        private readonly IRoleService roleService;

        public RegisterRolesCommandService(IRoleService roleService)
        {
            if (roleService is null)
                throw new ArgumentNullException(nameof(roleService));

            this.roleService = roleService;
        }

        public async Task ExecuteAsync(OnStartupNullObject command)
        {
            string[] roles = Enum.GetNames(typeof(RoleType));

            foreach (var role in roles)
            {
                bool roleExists = await roleService.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleService.CreateAsync(role);
                }
            }
        }
    }
}
