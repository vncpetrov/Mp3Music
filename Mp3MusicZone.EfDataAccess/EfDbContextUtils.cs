namespace Mp3MusicZone.EfDataAccess
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class EfDbContextUtils
    {
        public static void UseDatabaseMigration(DbContext context)
        {
            context.Database.Migrate();
        }       
    }
}
