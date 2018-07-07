namespace Mp3MusicZone.EfDataAccess
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.IO;

    public static class EfDbContextMigrateDatabase
    {
        public static void UseDatabaseMigration(DbContext context)
        {
            context.Database.Migrate();

            RawSqlString command = File.ReadAllText("../ErrorLogsTable.sql");
            context.Database.ExecuteSqlCommand(command);
        }
    }
}
