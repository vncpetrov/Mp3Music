namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Domain.Models;
    using Models;
    using System;

    public class UnhandledExceptionEntryEfRepository :
        EfRepository<UnhandledExceptionEntry, UnhandledExceptionEntryEf>
    {
        public UnhandledExceptionEntryEfRepository(MusicZoneDbContext context)
            : base(context)
        {
        }
    }
}
