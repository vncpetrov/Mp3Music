namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Domain.Models;
    using DomainServices.CommandServices.Admin.DemoteUserFromRole;
    using DomainServices.CommandServices.Admin.PromoteUserToRole;
    using DomainServices.Contracts;
    using DomainServices.QueryServices;
    using DomainServices.QueryServices.Admin.GetUsers;
    using Infrastructure.Extensions;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Mp3MusicZone.DomainServices.QueryServices.Users.GetUsersCount;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using ViewModels;
    using Web.Controllers;
    using Web.ViewModels.Shared;

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
            IEnumerable<User> users = null;
            GetUsers query = new GetUsers()
            {
                Page = page,
                SearchInfo = new SearchInfo(searchTerm)
            };

            string message = await this.CallServiceAsync(
                async () => users = await this.getUsers.ExecuteAsync(query));

            if (message != null)
            {
                return RedirectToAction(
                    nameof(HomeController.Index), "Home", new { area = "" })
                    .WithErrorMessage(message);
            }

            int usersCount = await this.getUsersCount.ExecuteAsync(
                new GetUsersCount()
                {
                    SearchInfo = new SearchInfo(searchTerm)
                });

            IEnumerable<UserListingViewModel> usersModel =
                Mapper.Map<IEnumerable<UserListingViewModel>>(users);

            SearchViewModel<PaginatedViewModel<UserListingViewModel>> model =
                new SearchViewModel<PaginatedViewModel<UserListingViewModel>>(
                    new PaginatedViewModel<UserListingViewModel>(
                        usersModel,
                        page,
                        2,
                        usersCount),
                    searchTerm,
                    "users");

            return View(model);
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
                if (message.Contains("do not have permissions"))
                {
                    return RedirectToAction(
                        nameof(HomeController.Index), "Home", new { area = "" })
                        .WithErrorMessage(message);
                }

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
                if (message.Contains("do not have permissions"))
                {
                    return RedirectToAction(
                        nameof(HomeController.Index), "Home", new { area = "" })
                        .WithErrorMessage(message);
                }

                return RedirectToAction(nameof(this.Index))
                    .WithErrorMessage(message);
            }

            return RedirectToAction(nameof(this.Index))
                .WithSuccessMessage($"User {model.Username} successfully demoted from {model.RoleName} role.");
        }

    }
}