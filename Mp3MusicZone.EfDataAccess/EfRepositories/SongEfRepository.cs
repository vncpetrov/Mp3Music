namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using Domain.Models;
    using Models;
    using System;

    public class SongEfRepository : EfRepository<Song, SongEf>
    {
        public SongEfRepository(MusicZoneDbContext context) 
            : base(context)
        {
        }
    }
}
