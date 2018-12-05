namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Common.Providers;
    using Controllers;
    using Domain.Contracts;
    using DomainServices.CommandServices.Songs.EditSong;
    using DomainServices.CommandServices.Songs.UploadSong;
    using DomainServices.CommandServicesAspects;
    using DomainServices.QueryServices.Songs.GetForEditById;
    using DomainServices.QueryServices.Songs.GetLastApproved;
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
    using EfDataAccess;
    using EfDataAccess.EfRepositories;
    using FileAccess;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Logging;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.DomainServices;
    using Mp3MusicZone.DomainServices.CommandServices.Songs.DeleteSong;
    using Mp3MusicZone.DomainServices.Contracts;
    using Mp3MusicZone.DomainServices.QueryServices.Songs.GetForDeleteById;
    using Mp3MusicZone.DomainServices.QueryServicesAspects;
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
        private readonly IDateTimeProvider dateTimeProvider;

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
            this.dateTimeProvider = new SystemDateTimeProvider();
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
            MusicZoneDbContext efDbContext = new MusicZoneDbContext(this.connectionString);
            // Similar as context.HttpContext.Response.RegisterForDispose method
            TrackDisposable(context, efDbContext);

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
                    return this.CreateHomeController(scope);

                case "AccountController":
                    return this.CreateAccountController(scope);

                case "ManageController":
                    return this.CreateManageController(scope);

                case "SongsController":
                    return this.CreateSongsController(scope);

                default:
                    throw new Exception("Unknown controller " + type.Name);
            }
        }

        private Controller CreateSongsController(Scope scope)
        {
            //return new SongsController(
            //    new TransactionCommandServiceDecorator<EditSong>(new EditSongCommandService(
            //        new SongEfRepository(this.CreateContext(scope)),
            //        new SongProvider("../Music"),
            //        this.CreateContext(scope))),
            //    new TransactionCommandServiceDecorator<UploadSong>(new UploadSongCommandService(
            //        new SongEfRepository(this.CreateContext(scope)),
            //        new SongProvider("../Music"),
            //        this.dateTimeProvider,
            //        this.CreateContext(scope))),
            //    new GetSongByIdQueryService(
            //        new SongEfRepository(this.CreateContext(scope))),
            //    new GetSongForPlayingQueryService(
            //        new SongProvider("../Music"),
            //        new SongEfRepository(this.CreateContext(scope))));

            return new SongsController(
                new PermissionCommandServiceDecorator<EditSong>(
                    new ServicePermissionChecker<EditSong>(
                        new UserPermissionChecker(
                            this.CreateUserRepository(scope),
                            this.CreateUserContext(scope))),
                    new TransactionCommandServiceDecorator<EditSong>(
                        new EditSongCommandService(
                            this.CreateSongRepository(scope),
                            this.CreateSongProvider(scope),
                            this.CreateContext(scope)))),

                new PermissionCommandServiceDecorator<UploadSong>(
                    new ServicePermissionChecker<UploadSong>(
                        new UserPermissionChecker(
                            this.CreateUserRepository(scope),
                            this.CreateUserContext(scope))),
                    new TransactionCommandServiceDecorator<UploadSong>(
                        new UploadSongCommandService(
                            this.CreateSongRepository(scope),
                            this.CreateSongProvider(scope),
                            this.dateTimeProvider,
                            this.CreateContext(scope)))),

                new PermissionCommandServiceDecorator<DeleteSong>(
                    new ServicePermissionChecker<DeleteSong>(
                        new UserPermissionChecker(
                            this.CreateUserRepository(scope),
                            this.CreateUserContext(scope))),
                    new TransactionCommandServiceDecorator<DeleteSong>(
                        new DeleteSongCommandService(
                            this.CreateSongRepository(scope),
                            this.CreateSongProvider(scope),
                            this.CreateContext(scope)))),

                this.CreatePermissionQueryService<GetSongForEditById, Song>(
                    new GetSongForEditByIdQueryService(
                        this.CreateSongRepository(scope)),
                    scope),

                this.CreatePermissionQueryService<GetSongForDeleteById, Song>(
                    new GetSongForDeleteByIdQueryService(
                        this.CreateSongRepository(scope)),
                    scope),

                new GetSongForPlayingQueryService(
                    this.CreateSongProvider(scope),
                    this.CreateSongRepository(scope)));
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

        private HomeController CreateHomeController(Scope scope)
            => new HomeController(
                    new GetLastApprovedSongsQueryService(
                        this.CreateSongRepository(scope)));

        private IUserPermissionChecker CreateUserPermissionChecker(Scope scope)
            => new UserPermissionChecker(
                this.CreateUserRepository(scope),
                this.CreateUserContext(scope));

        private PermissionQueryServiceDecorator<TQuery, TResult>
            CreatePermissionQueryService<TQuery, TResult>(
                IQueryService<TQuery, TResult> queryService,
                Scope scope)
            where TQuery : IQuery<TResult>
            => scope.Get(_ =>
                  new PermissionQueryServiceDecorator<TQuery, TResult>(
                      this.CreateServicePermissionChecker<TQuery>(scope), queryService));

        private ServicePermissionChecker<T> CreateServicePermissionChecker<T>(Scope scope)
            => scope.Get<ServicePermissionChecker<T>>(_ =>
                  new ServicePermissionChecker<T>(
                        new UserPermissionChecker(
                            this.CreateUserRepository(scope),
                            this.CreateUserContext(scope))));

        private SongProvider CreateSongProvider(Scope scope)
            => scope.Get<SongProvider>(_ =>
                  new SongProvider("../Music"));

        private AspNetUserContext CreateUserContext(Scope scope)
            => scope.Get<AspNetUserContext>(_ =>
                  new AspNetUserContext());

        private SongEfRepository CreateSongRepository(Scope scope)
            => scope.Get<SongEfRepository>(_ =>
                  new SongEfRepository(
                      this.CreateContext(scope)));

        private UserEfRepository CreateUserRepository(Scope scope)
            => scope.Get<UserEfRepository>(_ =>
                  new UserEfRepository(
                      this.CreateContext(scope)));

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
