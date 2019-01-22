namespace Mp3MusicZone.DomainServices.CommandServices.OnStartup.AddRolesPermissions
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Domain.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Common.Constants.Permissions;

    public class AddRolesPermissionsCommandService
        : ICommandService<OnStartupNullObject>
    {
        private readonly IEfRepository<Role> roleRepository;
        private readonly IEfRepository<Permission> permissionRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public AddRolesPermissionsCommandService(
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
            await this.AddAdministratorRolePermissionsAsync();
            await this.AddUploaderRolePermissionsAsync();
            await this.AddRegularUserRolePermissionsAsync();

            this.contextSaveChanges.SaveChanges();
        }

        private async Task AddAdministratorRolePermissionsAsync()
        {
            Role adminRole = await this.roleRepository
               .All(eagerLoading: true)
               .FirstAsync(r => r.Name.ToLower() == RoleType.Administrator
                                                            .ToString()
                                                            .ToLower());

            List<Permission> permissions = await this.permissionRepository.All()
                .ToListAsync();

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
        }

        private async Task AddUploaderRolePermissionsAsync()
        {
            Role uploaderRole = await this.roleRepository
                .All(eagerLoading: true)
                .FirstAsync(r => r.Name.ToLower() == RoleType.Uploader
                                                             .ToString()
                                                             .ToLower());

            if (uploaderRole.Permissions.Count == 6)
            {
                return;
            }

            uploaderRole.Permissions.Add(new Permission() { Id = GetUnapprovedSongs });
            uploaderRole.Permissions.Add(new Permission() { Id = EditSong });
            uploaderRole.Permissions.Add(new Permission() { Id = DeleteSong });
            uploaderRole.Permissions.Add(new Permission() { Id = ApproveSong });
            uploaderRole.Permissions.Add(new Permission() { Id = RejectSong });
            uploaderRole.Permissions.Add(new Permission() { Id = UploadSong });
            
            this.roleRepository.Update(uploaderRole);
        }

        private async Task AddRegularUserRolePermissionsAsync()
        {
            Role regularUserRole = await this.roleRepository
                .All(eagerLoading: true)
                .FirstAsync(r => r.Name.ToLower() == RoleType.RegularUser
                                                             .ToString()
                                                             .ToLower());

            if (regularUserRole.Permissions.Count == 1)
            {
                return;
            }

            regularUserRole.Permissions.Add(new Permission() { Id = UploadSong });

            this.roleRepository.Update(regularUserRole);
        }
    }
}
