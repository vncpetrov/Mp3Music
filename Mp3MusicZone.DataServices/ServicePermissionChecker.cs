namespace Mp3MusicZone.DomainServices
{
    using Domain.Attributes;
    using Domain.Contracts;
    using System;
    using System.Reflection;
    
    //Generic Type(T): command or query
    public class ServicePermissionChecker<T>
    {
        private static readonly string permissionId;
        private readonly IUserPermissionChecker permissionChecker;

        static ServicePermissionChecker()
        {
            PermissionAttribute permissionAttr =
                typeof(T).GetCustomAttribute<PermissionAttribute>();

            if (permissionAttr is null)
            {
                throw new InvalidOperationException($"The type {typeof(T).Name} is not marked with the [{typeof(PermissionAttribute).Name}]. Please define the permission the user need to execute this action.");
            }

            permissionId = permissionAttr.PermissionId;
        }

        public ServicePermissionChecker(IUserPermissionChecker permissionChecker)
        {
            if (permissionChecker is null)
                throw new ArgumentNullException(nameof(permissionChecker));

            this.permissionChecker = permissionChecker;
        }

        public void CheckPermissionForCurrentUser()
        {
            this.permissionChecker.CheckPermission(permissionId);
        }
    }
}
