namespace Mp3MusicZone.Web
{
    using Auth.Contracts;
    using EfDataAccess;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NLog;
    using NLog.Web;
    using System;

    using static Common.Constants.WebConstants;

    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            //string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { @"bin\" }, StringSplitOptions.None)[0];
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(projectPath)
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //string connectionString =
            //    configuration.GetConnectionString(ConnectionStringSectionName); 

            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                string connectionString = services.GetService<IConfiguration>()
                    .GetConnectionString(ConnectionStringSectionName);

                MusicZoneDbContext efDbContext = new MusicZoneDbContext(connectionString);
                EfDbContextMigrateDatabase.UseDatabaseMigration(efDbContext);

                IUserService userService = services.GetService<IUserService>();
                IRoleService roleService = services.GetService<IRoleService>();

                DataSeeder.Seed(userService, roleService);
            }

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseNLog();
    }
}
