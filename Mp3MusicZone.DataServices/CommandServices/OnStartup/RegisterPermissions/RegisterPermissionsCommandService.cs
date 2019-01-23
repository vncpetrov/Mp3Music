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
    using Common.Constants;
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

            List<Type> commands = Assembly.GetExecutingAssembly()
                 .GetTypes()
                 .Where(t => t.Namespace.Contains("CommandService")
                             && t.GetCustomAttribute<PermissionAttribute>() != null)
                 .ToList();

            foreach (var type in types)
            {
                // Check if Command and Query require the same Permission
                if (type.GetInterfaces()
                        .Any(i => i.IsGenericType
                                  && i.GetGenericTypeDefinition() == typeof(IQuery<>))
                    && commands.Any(c =>
                        c.GetCustomAttribute<PermissionAttribute>().PermissionId ==
                        type.GetCustomAttribute<PermissionAttribute>().PermissionId))
                {
                    continue;
                }

                string permissionName = type.Name;

                if (!existingPermissions.Any(p => p.Name == permissionName))
                {
                    this.permissionRepository.Add(
                        new Permission()
                        {
                            Id = (string)typeof(Permissions)
                                    .GetField(permissionName)
                                    .GetValue(null),
                            Name = permissionName
                        });
                }
            }

            this.contextSaveChanges.SaveChanges();
            await Task.CompletedTask;
        }
    }
}
