namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Domain.Models;
    using Models;
    using System;

    public class PermissionEfRepository : EfRepository<Permission, PermissionEf>
    {
        public PermissionEfRepository(MusicZoneDbContext context) 
            : base(context)
        {
        }
    }
}
