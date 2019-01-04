namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.EfDataAccess.Models;
    using System;

    public class AuditEntryEfRepository : EfRepository<AuditEntry, AuditEntryEf>
    {
        public AuditEntryEfRepository(MusicZoneDbContext context) 
            : base(context)
        {
        }
    }
}
