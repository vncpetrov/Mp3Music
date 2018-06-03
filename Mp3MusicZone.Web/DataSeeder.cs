namespace Mp3MusicZone.Web
{
    using Auth.Contracts;
    using Domain.Models.Enums;
    using EfDataAccess.Models;
    using System;
    using System.Threading.Tasks;

    public static class DataSeeder
    {
        public static void Seed(IUserService userService, IRoleService roleService)
        {
            if (userService is null) throw new ArgumentNullException(nameof(userService));
            if (roleService is null) throw new ArgumentNullException(nameof(roleService));

            SeedRoles(roleService);
            SeedAdministrator(userService);
        }

        private static void SeedRoles(IRoleService roleService)
        {
            Task.Run(async () =>
            {
                string[] roles = Enum.GetNames(typeof(Role));

                foreach (var role in roles)
                {
                    bool roleExists = await roleService.RoleExistsAsync(role);

                    if (!roleExists)
                    {
                        await roleService.CreateAsync(role);
                    }
                }
            })
            .GetAwaiter()
            .GetResult();
        }

        private static void SeedAdministrator(IUserService userService)
        {
            Task.Run(async () =>
            {
                string adminName = Role.Administrator.ToString();

                UserEf adminUser = await userService.FindByNameAsync(adminName);

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

            })
            .GetAwaiter()
            .GetResult();
        }
    }
}
