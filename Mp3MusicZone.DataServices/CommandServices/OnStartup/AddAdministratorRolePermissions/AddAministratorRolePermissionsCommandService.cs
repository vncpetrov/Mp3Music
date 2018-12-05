namespace Mp3MusicZone.DomainServices.CommandServices.OnStartup
    .AddAdministratorRolePermissions
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Domain.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddAministratorRolePermissionsCommandService
        : ICommandService<OnStartupNullObject>
    {
        private readonly IEfRepository<Role> roleRepository;
        private readonly IEfRepository<Permission> permissionRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public AddAministratorRolePermissionsCommandService(
            IEfRepository<Role> roleRepository,
            IEfRepository<Permission> permissionRepository,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (roleRepository is null)
                throw new ArgumentNullException(nameof(roleRepository));

            if (permissionRepository is null)
                throw new ArgumentNullException(nameof(permissionRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.roleRepository = roleRepository;
            this.permissionRepository = permissionRepository;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task ExecuteAsync(OnStartupNullObject command)
        {
            Role adminRole = this.roleRepository
                .All(eagerLoading: true)
                .First(r => r.Name.ToLower() == RoleType.Administrator
                                                        .ToString()
                                                        .ToLower());

            List<Permission> permissions = this.permissionRepository.All()
                .ToList();

            if (adminRole.Permissions.Count == permissions.Count)
            {
                return;
            }

            foreach (var permission in adminRole.Permissions)
            {
                Permission permissionToRemove =
                    permissions.Find(p => p.Id == permission.Id);

                permissions.Remove(permissionToRemove);
            }

            adminRole.Permissions = permissions;

            this.roleRepository.Update(adminRole);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
