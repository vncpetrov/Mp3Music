namespace Mp3MusicZone.Web
{
    using Auth.Contracts;
    using EfDataAccess;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
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
                EfDbContextUtils.UseDatabaseMigration(efDbContext);
                efDbContext.Database.ExecuteSqlCommand(@"IF OBJECT_ID(N'dbo.AdminLogs') IS NULL
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[AdminLogs] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Application] [nvarchar](50) NOT NULL,
		[LoggedOn] [datetime] NOT NULL,
		[Level] [nvarchar](50) NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[Logger] [nvarchar](250) NULL,
		[Callsite] [nvarchar](max) NULL,
		[Exception] [nvarchar](max) NULL,
	  CONSTRAINT [PK_dbo.AdminLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
");


                efDbContext.Database.ExecuteSqlCommand(@"IF OBJECT_ID(N'dbo.ErrorLogs') IS NULL
BEGIN
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[ErrorLogs] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Application] [nvarchar](50) NOT NULL,
		[LoggedOn] [datetime] NOT NULL,
		[Level] [nvarchar](50) NOT NULL,
		[Message] [nvarchar](max) NOT NULL,
		[Logger] [nvarchar](250) NULL,
		[Callsite] [nvarchar](max) NULL,
		[Exception] [nvarchar](max) NULL,
	  CONSTRAINT [PK_dbo.ErrorLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
		WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
");
                efDbContext.SaveChanges();
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
