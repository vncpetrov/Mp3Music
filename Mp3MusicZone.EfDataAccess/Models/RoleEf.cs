namespace Mp3MusicZone.EfDataAccess.Models
{
    using Microsoft.AspNetCore.Identity;
    using Models.MappingTables;
    using System;
    using System.Collections.Generic;

    public class RoleEf : IdentityRole
    {
        public ICollection<RolePermission> Permissions { get; set; } =
            new HashSet<RolePermission>();

        public ICollection<UserRole> Users { get; set; } =
            new HashSet<UserRole>();
    }
}
