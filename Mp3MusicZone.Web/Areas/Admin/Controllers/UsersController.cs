namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using DomainServices.CommandServices.Admin.PromoteUserToRole;
    using DomainServices.Contracts;
    using DomainServices.QueryServices;
    using DomainServices.QueryServices.Admin.GetUsers;
    using DomainServices.QueryServices.Users.GetUsersCount;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels;
    using Web.Components;
    using Web.ViewModels;
    using Web.ViewModels.Shared;

    using static Common.Constants.WebConstants;

    [Authorize]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ICommandService<PromoteUserToRole> promoteUser;
        private readonly ICommandService<DemoteUserFromRole> demoteUser;

        private readonly IQueryService<GetUsers, IEnumerable<User>> getUsers;
        private readonly IQueryService<GetUsersCount, int> getUsersCount;

        public UsersController(
            ICommandService<PromoteUserToRole> promoteUser,
            ICommandService<DemoteUserFromRole> demoteUser,

            IQueryService<GetUsers, IEnumerable<User>> getUsers,
            IQueryService<GetUsersCount, int> getUsersCount)
        {
            if (promoteUser is null)
                throw new ArgumentNullException(nameof(promoteUser));

            if (demoteUser is null)
                throw new ArgumentNullException(nameof(demoteUser));

            if (getUsers is null)
                throw new ArgumentNullException(nameof(getUsers));

            if (getUsersCount is null)
                throw new ArgumentNullException(nameof(getUsersCount));

            this.promoteUser = promoteUser;
            this.demoteUser = demoteUser;
            this.getUsers = getUsers;
            this.getUsersCount = getUsersCount;
        }

        public async Task<IActionResult> Index(int page = 1, string searchTerm = null)
        {
            GetUsers getUsersQuery = new GetUsers()
            {
                PageInfo = new PageInfo(page, DefaultPageSize),
                SearchInfo = new SearchInfo(searchTerm)
            };

            IEnumerable<User> users = await this.getUsers.ExecuteAsync(getUsersQuery);

            GetUsersCount getUsersCountQuery = new GetUsersCount()
            {
                SearchInfo = new SearchInfo(searchTerm)
            };

            int usersCount = await this.getUsersCount.ExecuteAsync(getUsersCountQuery);

            IEnumerable<UserListingViewModel> usersModel =
                Mapper.Map<IEnumerable<UserListingViewModel>>(users);

            SearchViewModel<PaginatedViewModel<UserListingViewModel>> model =
                ViewModelFactory.CreateSearchPaginatedViewModel<UserListingViewModel>(
                    usersModel,
                    page,
                    DefaultPageSize,
                    usersCount,
                    searchTerm,
                    "users");

            return View(model);
        }

        [AjaxOnly]
        [AllowAnonymous]
        public async Task<IActionResult> FilteredUsersAjax(string searchTerm = null)
        {
            IEnumerable<User> users = null;

            GetUsers query = new GetUsers()
            {
                PageInfo = new PageInfo(1, DefaultPageSize),
                SearchInfo = new SearchInfo(searchTerm)
            };

            string message = await this.CallServiceAsync(
                async () => users = await this.getUsers.ExecuteAsync(query));


            IEnumerable<UserListingViewModel> model =
                Mapper.Map<IEnumerable<UserListingViewModel>>(users);

            return PartialView("_UserListing", model);
        }

        [AjaxOnly]
        [AllowAnonymous]
        public async Task<IActionResult> PaginationAjax(string searchTerm)
        {
            GetUsersCount query = new GetUsersCount()
            {
                SearchInfo = new SearchInfo(searchTerm)
            };

            int songsCount = await this.getUsersCount.ExecuteAsync(query);

            return ViewComponent(
                typeof(PaginationComponent),
                new
                {
                    pageInfo = new PaginatedViewModel<string>(
                        null, 1, DefaultPageSize, songsCount),
                    searchTerm = searchTerm,
                    actionToCall = "index"
                });
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> PromoteUser(UserRoleViewModel model)
        {
            PromoteUserToRole command = Mapper.Map<PromoteUserToRole>(model);

            string message = await this.CallServiceAsync(
                async () => await this.promoteUser.ExecuteAsync(command));

            if (message != null)
            {
                return RedirectToAction(nameof(this.Index))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(this.Index))
                .WithSuccessMessage($"User {model.Username} successfully promoted to {model.RoleName} role.");
        }


        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> DemoteUser(UserRoleViewModel model)
        {
            string loggedUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            DemoteUserFromRole command = new DemoteUserFromRole()
            {
                UserId = model.UserId,
                RoleName = model.RoleName,
                LoggedUserId = loggedUserId
            };

            string message = await this.CallServiceAsync(
                async () => await this.demoteUser.ExecuteAsync(command));

            if (message != null)
            {
                return RedirectToAction(nameof(this.Index))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(this.Index))
                .WithSuccessMessage($"User {model.Username} successfully demoted from {model.RoleName} role.");
        }
    }
}