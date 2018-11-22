namespace Mp3MusicZone.Web.Infrastructure.AspNetOnStartupCommandServices.RegisterAdministrator
{
    using Auth.Contracts;
    using Domain.Models.Enums;
    using DomainServices.CommandServices.OnStartup;
    using DomainServices.Contracts;
    using EfDataAccess.Models;
    using System;
    using System.Threading.Tasks;

    public class RegisterAdministratorCommandService : ICommandService<OnStartupNullObject>
    {
        private readonly IUserService userService;

        public RegisterAdministratorCommandService(IUserService userService)
        {
            if (userService is null)
                throw new ArgumentNullException(nameof(userService));

            this.userService = userService;
        }

        public async Task ExecuteAsync(OnStartupNullObject command)
        {
            string adminName = "Administrator";

            UserEf adminUser = this.userService.FindByNameAsync(adminName).Result;

            if (adminUser is null)
            {
                adminUser = new UserEf()
                {
                    UserName = adminName,
                    Email = "mp3musiczone.info@gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Mp3MusicZone",
                    LastName = adminName,
                    Birthdate = new DateTime(1990, 4, 16),
                    Genre = GenreType.Male,
                };

                await userService.CreateAsync(adminUser, "Test12");

                await userService.AddToRoleAsync(adminUser, adminName);
            }
        }
    }
}
