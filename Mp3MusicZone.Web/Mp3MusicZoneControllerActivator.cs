namespace Mp3MusicZone.Web
{
    using Auth;
    using Auth.Contracts;
    using Common.Providers;
    using Controllers;
    using Domain.Contracts;
    using Domain.Models;
    using DomainServices;
    using DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using DomainServices.CommandServices.Admin.PromoteUserToRole;
    using DomainServices.CommandServices.Songs.DeleteSong;
    using DomainServices.CommandServices.Songs.EditSong;
    using DomainServices.CommandServices.Songs.UploadSong;
    using DomainServices.CommandServices.Uploader.ApproveSong;
    using DomainServices.CommandServices.Uploader.RejectSong;
    using DomainServices.CommandServicesAspects;
    using DomainServices.Contracts;
    using DomainServices.QueryServices.Admin.GetUsers;
    using DomainServices.QueryServices.Songs.GetForDeleteById;
    using DomainServices.QueryServices.Songs.GetForEditById;
    using DomainServices.QueryServices.Songs.GetLastApproved;
    using DomainServices.QueryServices.Songs.GetSongForPlaying;
    using DomainServices.QueryServices.Songs.GetSongs;
    using DomainServices.QueryServices.Songs.GetSongsCount;
    using DomainServices.QueryServices.Uploader.GetUnapprovedSongs;
    using DomainServices.QueryServices.Users.GetUsersCount;
    using DomainServices.QueryServicesAspects;
    using EfDataAccess;
    using EfDataAccess.EfRepositories;
    using FileAccess;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Logging;
    using Mp3MusicZone.DomainServices.CommandServices.Songs.IncrementSongListenings;
    using Mp3MusicZone.Web.FacadeServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;
    using Web.Areas.Admin.Controllers;

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

            Controller controller = this.CheckAreasControllers(type, scope);

            if (controller is null)
            {
                controller = this.CheckNormalControllers(type, scope);
            }

            return controller;
        }

        private Controller CreateUploaderSongsController(Scope scope)
        {
            return new Areas.Uploader.Controllers.SongsController(
                this.CreatePermissionCommandService<ApproveSong>(
                    this.CreateTransactionCommandService<ApproveSong>(
                        this.CreateAuditingCommandService<ApproveSong>(
                            new ApproveSongCommandService(
                                this.CreateSongRepository(scope),
                                this.CreateContext(scope)),
                            scope),
                        scope),
                    scope),

                this.CreatePermissionCommandService<RejectSong>(
                    this.CreateTransactionCommandService<RejectSong>(
                        this.CreateAuditingCommandService<RejectSong>(
                            new RejectSongCommandService(
                                this.CreateSongRepository(scope),
                                this.CreateSongProvider(scope),
                                this.CreateContext(scope)),
                            scope),
                        scope),
                    scope),

                this.CreatePermissionQueryService<GetUnapprovedSongs, IEnumerable<Song>>(
                    new GetUnapprovedSongsQueryService(
                        this.CreateSongRepository(scope)),
                    scope),

                new GetSongsCountQueryService(
                    this.CreateSongRepository(scope)));
        }

        private Controller CreateAdminUsersController(Scope scope)
        {
            return new UsersController(
                this.CreatePermissionCommandService<PromoteUserToRole>(
                    this.CreateTransactionCommandService<PromoteUserToRole>(
                        this.CreateAuditingCommandService<PromoteUserToRole>(
                            new PromoteUserToRoleCommandService(
                                 this.CreateUserRepository(scope),
                                 this.CreateRoleRepository(scope),
                                 this.CreateContext(scope)),
                            scope),
                        scope),
                    scope),

                this.CreatePermissionCommandService<DemoteUserFromRole>(
                    this.CreateTransactionCommandService<DemoteUserFromRole>(
                        this.CreateAuditingCommandService<DemoteUserFromRole>(
                            new DemoteUserFromRoleCommandService(
                                 this.CreateUserRepository(scope),
                                 this.CreateRoleRepository(scope),
                                 this.CreateContext(scope)),
                            scope),
                        scope),
                    scope),
                
                this.CreatePermissionQueryService<GetUsers, IEnumerable<User>>(
                    new GetUsersQueryService(
                        this.CreateUserRepository(scope)),
                    scope),

                new GetUsersCountQueryService(
                    this.CreateUserRepository(scope)));
        }

        private Controller CreateSongsController(Scope scope)
        {
            return new SongsController(
                this.CreatePermissionCommandService<EditSong>(
                     this.CreateTransactionCommandService<EditSong>(
                         this.CreateAuditingCommandService<EditSong>(
                             new EditSongCommandService(
                                 this.CreateSongRepository(scope),
                                 this.CreateSongProvider(scope),
                                 this.CreateContext(scope)),
                             scope),
                         scope),
                     scope),


                this.CreatePermissionCommandService<UploadSong>(
                     this.CreateTransactionCommandService<UploadSong>(
                         this.CreateAuditingCommandService<UploadSong>(
                             new UploadSongCommandService(
                                 this.CreateSongRepository(scope),
                                 this.CreateSongProvider(scope),
                                 this.dateTimeProvider,
                                 this.CreateContext(scope)),
                             scope),
                         scope),
                     scope),

                this.CreatePermissionCommandService<DeleteSong>(
                     this.CreateTransactionCommandService<DeleteSong>(
                         this.CreateAuditingCommandService<DeleteSong>(
                             new DeleteSongCommandService(
                                 this.CreateSongRepository(scope),
                                 this.CreateSongProvider(scope),
                                 this.CreateContext(scope)),
                             scope), 
                         scope), 
                     scope),


                this.CreatePermissionQueryService<GetSongForEditById, Song>(
                    new GetSongForEditByIdQueryService(
                        this.CreateSongRepository(scope)),
                    scope),

                this.CreatePermissionQueryService<GetSongForDeleteById, Song>(
                    new GetSongForDeleteByIdQueryService(
                        this.CreateSongRepository(scope)),
                    scope),

                new GetSongsCountQueryService(
                    this.CreateSongRepository(scope)),

                new GetSongsQueryService(
                        this.CreateSongRepository(scope)),

                new SongPlayer(
                    new IncrementSongListeningsCommandService(
                        this.CreateSongRepository(scope),
                        this.CreateContext(scope)),
                    new GetSongForPlayingQueryService(
                        this.CreateSongProvider(scope),
                        this.CreateSongRepository(scope))));
        }

        private ManageController CreateManageController(Scope scope)
        {
            ILogger<ManageController> logger = (ILogger<ManageController>)this.accessor.HttpContext
                .RequestServices
                .GetService(typeof(ILogger<ManageController>));

            return new ManageController(
                this.CreateUserRepository(scope),
                this.CreateUserService(scope),
                this.CreateSignInService(scope),
                this.emailSender,
                logger,
                UrlEncoder.Default);
        }

        private Controller CreateAccountController(Scope scope)
        {
            ILogger<AccountController> logger =
                (ILogger<AccountController>)this.accessor
                .HttpContext
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
                      this.CreateServicePermissionChecker<TQuery>(scope),
                      queryService));

        private PermissionCommandServiceDecorator<TCommand>
            CreatePermissionCommandService<TCommand>(
                ICommandService<TCommand> commandService,
                Scope scope)
            => scope.Get(_ =>
                  new PermissionCommandServiceDecorator<TCommand>(
                      this.CreateServicePermissionChecker<TCommand>(scope),
                      commandService));

        private TransactionCommandServiceDecorator<TCommand>
            CreateTransactionCommandService<TCommand>(
                ICommandService<TCommand> commandService,
                Scope scope)
            => scope.Get(_ =>
                  new TransactionCommandServiceDecorator<TCommand>(
                    commandService));

        private AuditingCommandServiceDecorator<TCommand>
            CreateAuditingCommandService<TCommand>(
                ICommandService<TCommand> commandService,
                Scope scope)
            => scope.Get(_ =>
                  new AuditingCommandServiceDecorator<TCommand>(
                      this.CreateRepository<AuditEntryEfRepository>(scope),
                      this.CreateContext(scope),
                      this.dateTimeProvider,
                      this.CreateUserContext(scope),
                      commandService));

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

        private T CreateRepository<T>(Scope scope)
            where T : class
            => scope.Get<T>(_ =>
                  (T)Activator.CreateInstance(typeof(T), this.CreateContext(scope)));

        private RoleEfRepository CreateRoleRepository(Scope scope)
            => scope.Get<RoleEfRepository>(_ =>
                  new RoleEfRepository(
                      this.CreateContext(scope)));

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

        private static void TrackDisposable(
            ControllerContext context,
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

        private Controller CheckNormalControllers(Type type, Scope scope)
        {
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

        private Controller CheckAreasControllers(Type type, Scope scope)
        {
            if (type.FullName.Contains("Areas"))
            {
                if (type.FullName.Contains("Uploader"))
                {
                    switch (type.Name)
                    {
                        case "SongsController":
                            return this.CreateUploaderSongsController(scope);

                        default:
                            throw new Exception("Unknown controller " + type.Name);
                    }
                }
                else if (type.FullName.Contains("Admin"))
                {
                    switch (type.Name)
                    {
                        case "UsersController":
                            return this.CreateAdminUsersController(scope);

                        default:
                            throw new Exception("Unknown controller " + type.Name);
                    }
                }
            }

            return null;
        }
    }
}