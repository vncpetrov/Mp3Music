namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Domain.Models;
    using Models;
    using System;

    public class AuditEntryEfRepository : EfRepository<AuditEntry, AuditEntryEf>
    {
        public AuditEntryEfRepository(MusicZoneDbContext context)
            : base(context)
        {
        }
    }
}
