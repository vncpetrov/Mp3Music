namespace Mp3MusicZone.EfDataAccess
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.IO;

    public static class EfDbContextUtils
    {
        public static void UseDatabaseMigration(DbContext context)
        {
            context.Database.Migrate();
        }

        //public static void ExecuteSqlFile(DbContext context, string path)
        //{
        //    RawSqlString command = File.ReadAllText(path);
        //    context.Database.ExecuteSqlCommand(command);
        //}
    }
}
