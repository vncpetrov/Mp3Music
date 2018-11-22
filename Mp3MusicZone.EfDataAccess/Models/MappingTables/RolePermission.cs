namespace Mp3MusicZone.EfDataAccess.Models.MappingTables
{
    using System;

    public class RolePermission
    {
        public string RoleId { get; set; }
        public RoleEf Role { get; set; }

        public string PermissionId { get; set; }
        public PermissionEf Permission { get; set; }
    }
}
