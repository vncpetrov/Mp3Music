namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Domain.Models;
    using Models;
    using System;

    public class PerformanceEntryEfRepository : EfRepository<PerformanceEntry, PerformanceEntryEf>
    {
        public PerformanceEntryEfRepository(MusicZoneDbContext efDbContext) 
            : base(efDbContext)
        {
        }
    }
}
