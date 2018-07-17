namespace Mp3MusicZone.EfDataAccess
{
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class EfDbContextUtils
    {
        public static void UseDatabaseMigration(DbContext context)
        {
            context.Database.Migrate();

            CreateErrorLogsTable(context);
            CreateAdminLogsTable(context);
        }

        private static void CreateAdminLogsTable(DbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF OBJECT_ID(N'dbo.AdminLogs') IS NULL
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
        }

        private static void CreateErrorLogsTable(DbContext context)
        {
            context.Database.ExecuteSqlCommand(@"IF OBJECT_ID(N'dbo.ErrorLogs') IS NULL
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
        }
    }
}
