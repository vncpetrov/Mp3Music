namespace Mp3MusicZone.DomainServices.CommandServices.OnStartup.RegisterPermissions
{
    using Domain.Attributes;
    using Domain.Contracts;
    using Domain.Models;
    using DomainServices.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    public class RegisterPermissionsCommandService : ICommandService<OnStartupNullObject>
    {
        private readonly IEfRepository<Permission> permissionRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;

        public RegisterPermissionsCommandService(
            IEfRepository<Permission> permissionRepository,
            IEfDbContextSaveChanges contextSaveChanges)
        {
            if (permissionRepository is null)
                throw new ArgumentNullException(nameof(permissionRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            this.permissionRepository = permissionRepository;
            this.contextSaveChanges = contextSaveChanges;
        }

        public async Task ExecuteAsync(OnStartupNullObject command)
        {
            List<Type> types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute<PermissionAttribute>() != null)
                .ToList();

            List<Permission> existingPermissions = this.permissionRepository
                .All()
                .ToList();

            foreach (var type in types)
            {
                string permissionName = type.Name;

                if (!existingPermissions.Any(p => p.Name == permissionName))
                {
                    this.permissionRepository.Add(
                        new Permission()
                        {
                            Name = permissionName
                        });
                }
            }

            this.contextSaveChanges.SaveChanges();

            await Task.CompletedTask;
        }
    }
}
