namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using AspNetOnStartupCommandServices.RegisterAdministrator;
    using AspNetOnStartupCommandServices.RegisterRoles;
    using Auth.Contracts;
    using DomainServices.CommandServices.OnStartup;
    using DomainServices.CommandServices.OnStartup.AddAdministratorRolePermissions;
    using DomainServices.CommandServices.OnStartup.RegisterPermissions;
    using DomainServices.Contracts;
    using EfDataAccess;
    using EfDataAccess.EfRepositories;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    using static Common.Constants.WebConstants;

    public static class WebHostExtensions
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                string connectionString = services.GetService<IConfiguration>()
                    .GetConnectionString(ConnectionStringSectionName);

                MusicZoneDbContext efDbContext = new MusicZoneDbContext(connectionString);
                //efDbContext.Database.EnsureDeleted();
                EfDbContextUtils.UseDatabaseMigration(efDbContext);
                //efDbContext.Database.Migrate();
            }

            return webHost;
        }

        public static async Task<IWebHost> SeedDatabase(this IWebHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                string connectionString = services.GetService<IConfiguration>()
                    .GetConnectionString(ConnectionStringSectionName);

                MusicZoneDbContext efDbContext = new MusicZoneDbContext(connectionString);

                IUserService userService = services.GetService<IUserService>();
                IRoleService roleService = services.GetService<IRoleService>();

                //DataSeeder.Seed(userService, roleService);

                ICommandService<OnStartupNullObject> startupCommandServices =
                    new CompositeOnStartupCommandService(
                        new ICommandService<OnStartupNullObject>[]
                        {
                            new RegisterPermissionsCommandService(
                                new PermissionEfRepository(efDbContext),
                                efDbContext),
                            new RegisterRolesCommandService(roleService),
                            new RegisterAdministratorCommandService(userService),
                            new AddAministratorRolePermissionsCommandService(
                                new RoleEfRepository(efDbContext),
                                new PermissionEfRepository(efDbContext),
                                efDbContext)
                        });

                await startupCommandServices.ExecuteAsync(new OnStartupNullObject());
            }

            return webHost;
        }
    }
}
