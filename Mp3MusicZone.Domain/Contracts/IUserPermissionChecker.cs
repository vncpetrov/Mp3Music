namespace Mp3MusicZone.Domain.Contracts
{
    using System;

    public interface IUserPermissionChecker
    {
        void CheckPermission(string permissionId);
    }
}