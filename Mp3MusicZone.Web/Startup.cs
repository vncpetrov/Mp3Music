namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Auth.Identity;
    using AutoMapper;
    using EfDataAccess;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    using static Common.Constants.WebConstants;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string connectionString =
                Configuration.GetConnectionString(ConnectionStringSectionName);

            services.AddDbContext<MusicZoneDbContext>(
                options => options.UseSqlServer(connectionString));

            //services.AddScoped<Mp3MusicZoneDbContext>(c => new Mp3MusicZoneDbContext(Configuration.GetConnectionString("Mp3MusicZoneConnectionString")));

            services.AddIdentity<UserEf, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<MusicZoneDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddAutoMapper();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var emailSettings = Configuration.GetSection("EmailSettings").Get<EmailSettings>();

            IControllerActivator activator = new Mp3MusicZoneControllerActivator(
                    connectionString,
                    new HttpContextAccessor(),
                    emailSettings);

            services.AddSingleton<IControllerActivator>(activator);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
