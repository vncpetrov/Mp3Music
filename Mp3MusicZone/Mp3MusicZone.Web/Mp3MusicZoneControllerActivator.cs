namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Domain.Contracts;
    using EfDataAccess;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Text.Encodings.Web;
    using Web.Controllers;

    public class Mp3MusicZoneControllerActivator : IControllerActivator
    {
        private readonly string connectionString;
        private readonly IHttpContextAccessor accessor;

        public Mp3MusicZoneControllerActivator(string connectionString,
        IHttpContextAccessor accessor)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (accessor is null)
            {
                throw new ArgumentNullException(nameof(accessor));
            }

            this.connectionString = connectionString;
            this.accessor = accessor;
        }

        public object Create(ControllerContext controllerContext)
        {
            Type type = controllerContext.ActionDescriptor
                .ControllerTypeInfo
                .AsType();

            return this.Create(type);
        }

        public void Release(ControllerContext context, object controller)
        {
        }

        private Controller Create(Type type)
        {
            MusicZoneDbContext efDbContext = new MusicZoneDbContext(connectionString);

            // can be optimized - lazy loading?
            IUserService userService = (IUserService)this.accessor.HttpContext
               .RequestServices
               .GetService(typeof(IUserService));

            ISignInService signInService = (ISignInService)this.accessor.HttpContext
                .RequestServices
                .GetService(typeof(ISignInService));

            IEmailSenderService emailSender = new EmailSenderService(
                new EmailSettings());

            switch (type.Name)
            {
                case "HomeController":
                    return this.CreateHomeController();

                case "AccountController":
                    return this.CreateAccountController(
                        userService,
                        signInService,
                        emailSender);

                case "ManageController":
                    return this.CreateManageController(
                        userService,
                        signInService,
                        emailSender);

                default:
                    throw new Exception("Unknown controller " + type.Name);
            }
        }

        private ManageController CreateManageController(IUserService userService,
            ISignInService signInService,
            IEmailSenderService emailSender)
        { 
            ILogger<ManageController> logger = (ILogger<ManageController>)this.accessor.HttpContext
                .RequestServices
                .GetService(typeof(ILogger<ManageController>));

            return new ManageController(userService, signInService, emailSender, logger, UrlEncoder.Default);
        }

        private Controller CreateAccountController(IUserService userService,
            ISignInService signInService,
            IEmailSenderService emailSender)
        {
            ILogger<AccountController> logger = (ILogger<AccountController>)this.accessor.HttpContext
            .RequestServices
            .GetService(typeof(ILogger<AccountController>));

            return new AccountController(
               userService,
               signInService,
               emailSender,
               logger);
        }

        private HomeController CreateHomeController()
        {
            return new HomeController();
        }
    }
}
