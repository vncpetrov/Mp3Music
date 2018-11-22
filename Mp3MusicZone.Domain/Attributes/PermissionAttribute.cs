namespace Mp3MusicZone.Domain.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string permissionId)
        {
            this.PermissionId = PermissionId;
        }

        public string PermissionId { get; }
    }
}
