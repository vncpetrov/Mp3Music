using Microsoft.Extensions.Configuration;
using Mp3MusicZone.EfDataAccess;
using NUnit.Framework;
using System;

using static Mp3MusicZone.Common.Constants.WebConstants;

[SetUpFixture]
public class TestsInitializer
{
    public static string ConnectionString { get; private set; }

    [OneTimeSetUp]
    public void AssemblyInit()
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(projectPath)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString =
            configuration.GetConnectionString(ConnectionStringSectionName);
        TestsInitializer.ConnectionString = connectionString;

        MusicZoneDbContext context = new MusicZoneDbContext(connectionString);
        EfDbContextMigrateDatabase.UseDatabaseMigration(context);
    }
}
