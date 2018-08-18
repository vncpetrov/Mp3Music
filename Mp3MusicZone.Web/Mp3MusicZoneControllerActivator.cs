﻿namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Controllers;
    using Domain.Contracts;
    using EfDataAccess;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;

    public class Mp3MusicZoneControllerActivator : IControllerActivator
    {
        private readonly string connectionString;
        private readonly IHttpContextAccessor accessor;
        private readonly EmailSettings emailSettings;

        // Singletons
        private readonly IEmailSenderService emailSender;

        public Mp3MusicZoneControllerActivator(
            string connectionString,
            IHttpContextAccessor accessor,
            EmailSettings emailSettings)
        {
            if (connectionString is null)
                throw new ArgumentNullException(nameof(connectionString));

            if (accessor is null)
                throw new ArgumentNullException(nameof(accessor));

            if (emailSettings is null)
                throw new ArgumentNullException(nameof(emailSettings));

            this.connectionString = connectionString;
            this.accessor = accessor;
            this.emailSettings = emailSettings;

            this.emailSender = new EmailSenderService(this.emailSettings);
        }

        public object Create(ControllerContext controllerContext)
        {
            Type type = controllerContext.ActionDescriptor
                .ControllerTypeInfo
                .AsType();

            return this.Create(type, controllerContext);
        }

        public void Release(ControllerContext context, object controller)
        {
            IList<IDisposable> disposables =
                (IList<IDisposable>)context.HttpContext.Items["Disposables"];

            if (disposables != null)
            {
                IEnumerable<IDisposable> reversed = disposables.Reverse();

                foreach (var disposable in reversed)
                {
                    disposable.Dispose();
                }
            }
        }

        private Controller Create(Type type, ControllerContext context)
        {
            //MusicZoneDbContext efDbContext = new MusicZoneDbContext(this.connectionString);
            //// Similar as context.HttpContext.Response.RegisterForDispose method
            //TrackDisposable(context, efDbContext);

            Scope scope = new Scope();
            TrackDisposable(context, scope);

            // can be optimized - lazy loading?
            //IUserService userService = (IUserService)this.accessor.HttpContext
            //   .RequestServices
            //   .GetService(typeof(IUserService));

            //ISignInService signInService = (ISignInService)this.accessor.HttpContext
            //    .RequestServices
            //    .GetService(typeof(ISignInService));

            //should be in singleton scope?
            //IEmailSenderService emailSender = new EmailSenderService(this.emailSettings);

            switch (type.Name)
            {
                case "HomeController":
                    return this.CreateHomeController();

                case "AccountController":
                    return this.CreateAccountController(scope);

                case "ManageController":
                    return this.CreateManageController(scope);

                default:
                    throw new Exception("Unknown controller " + type.Name);
            }
        } 

        private ManageController CreateManageController(Scope scope)
        {
            ILogger<ManageController> logger = (ILogger<ManageController>)this.accessor.HttpContext
                .RequestServices
                .GetService(typeof(ILogger<ManageController>));

            return new ManageController(
                this.CreateUserService(scope),
                this.CreateSignInService(scope),
                this.emailSender,
                logger,
                UrlEncoder.Default);
        }

        private Controller CreateAccountController(Scope scope)
        {
            ILogger<AccountController> logger = (ILogger<AccountController>)this.accessor.HttpContext
            .RequestServices
            .GetService(typeof(ILogger<AccountController>));

            return new AccountController(
               this.CreateUserService(scope),
               this.CreateSignInService(scope),
               this.emailSender,
               logger);
        }

        private HomeController CreateHomeController()
        {
            return new HomeController();
        }

        private MusicZoneDbContext CreateContext(Scope scope)
            => scope.Get<MusicZoneDbContext>(_ =>
                  new MusicZoneDbContext(
                      this.connectionString));

        private ISignInService CreateSignInService(Scope scope)
            => scope.Get<ISignInService>(_ =>
                (ISignInService)this.accessor.HttpContext
                    .RequestServices
                    .GetService(typeof(ISignInService)));

        private IUserService CreateUserService(Scope scope)
            => scope.Get<IUserService>(_ =>
                (IUserService)this.accessor.HttpContext
                    .RequestServices
                    .GetService(typeof(IUserService)));

        private static void TrackDisposable(ControllerContext context,
            IDisposable disposable)
        {
            IDictionary<object, object> contextItems = context.HttpContext.Items;

            if (!contextItems.ContainsKey("Disposables"))
            {
                contextItems["Disposables"] = new List<IDisposable>();
            }

            IList<IDisposable> disposableItems =
                (IList<IDisposable>)contextItems["Disposables"];
            disposableItems.Add(disposable);

        }
    }
}
