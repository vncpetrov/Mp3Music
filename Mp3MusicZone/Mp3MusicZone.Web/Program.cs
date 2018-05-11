﻿namespace Mp3MusicZone.Web
{
    using Auth.Contracts;
    using EfDataAccess;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = BuildWebHost(args);

            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString =
                configuration.GetConnectionString("MusicZoneConnectionString");
            
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                MusicZoneDbContext efDbContext = new MusicZoneDbContext(connectionString);
                EfDbContextMigrateDatabase.UseDatabaseMigration(efDbContext);

                IServiceProvider services = scope.ServiceProvider;
                
                IUserService userService = services.GetService<IUserService>();
                IRoleService roleService = services.GetService<IRoleService>();

                DataSeeder.Seed(userService, roleService);
            }

            webHost.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
