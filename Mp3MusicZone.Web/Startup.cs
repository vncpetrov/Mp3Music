namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Auth.Identity;
    using AutoMapper;
    using Domain.Contracts;
    using Domain.Models;
    using DomainServices;
    using EfDataAccess;
    using EfDataAccess.EfRepositories;
    using EfDataAccess.Models;
    using Infrastructure.Mappings;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using Web.Infrastructure;
    using System;

    using static Common.Constants.WebConstants;
    using Mp3MusicZone.Web.Infrastructure.Filters;
    using Mp3MusicZone.Common.Providers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionString = Configuration.GetConnectionString(ConnectionStringSectionName);

            services.AddDbContext<MusicZoneDbContext>(
                options => options.UseSqlServer(connectionString));

            //services.AddScoped<MusicZoneDbContext>(c => new MusicZoneDbContext(Configuration.GetConnectionString("MusicZoneConnectionString")));

            services.AddIdentity<UserEf, RoleEf>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<MusicZoneDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IUserContext, AspNetUserContext>();
            services.AddScoped<IEfRepository<User>, UserEfRepository>();
            services.AddScoped<IUserPermissionChecker, UserPermissionChecker>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IRoleService, RoleService>();

            // unhandled exceptions logger
            services.AddScoped<IEfDbContextSaveChanges>(
                x => x.GetService<MusicZoneDbContext>());

            services.AddScoped<
                IEfRepository<UnhandledExceptionEntry>, UnhandledExceptionEntryEfRepository>();

            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IExceptionLogger, UnhandledExceptionLogger>();

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add<CommonExceptionFilter>(1);
                options.Filters.Add<UnhandledExceptionFilter>(0);
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var emailSettings = Configuration.GetSection("EmailSettings")
                .Get<EmailSettings>();

            IControllerActivator activator = new Mp3MusicZoneControllerActivator(
                    connectionString,
                    new HttpContextAccessor(),
                    emailSettings);

            services.AddSingleton<IControllerActivator>(activator);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            GlobalDiagnosticsContext.Set("connectionString",
                Configuration.GetConnectionString(ConnectionStringSectionName));

            LogManager.ThrowExceptions = true;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }
    }
}
