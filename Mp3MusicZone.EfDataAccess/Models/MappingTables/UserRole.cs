namespace Mp3MusicZone.EfDataAccess.Models.MappingTables
{
    using Microsoft.AspNetCore.Identity;
    using System;

    public class UserRole : IdentityUserRole<string>
    {
        public RoleEf Role { get; set; }
        
        public UserEf User { get; set; }
    }
}
