using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mp3MusicZone.Domain.Models;
using Mp3MusicZone.DomainServices.CommandServices.Admin.DemoteUserFromRole;
using Mp3MusicZone.DomainServices.CommandServices.Admin.PromoteUserToRole;
using Mp3MusicZone.DomainServices.Contracts;
using Mp3MusicZone.DomainServices.QueryServices.Admin.GetUsers;
using Mp3MusicZone.Web.Areas.Admin.ViewModels;
using Mp3MusicZone.Web.Controllers;
using Mp3MusicZone.Web.Infrastructure.Extensions;
using Mp3MusicZone.Web.Infrastructure.Filters;

namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ICommandService<PromoteUserToRole> promoteUser;
        private readonly ICommandService<DemoteUserFromRole> demoteUser;

        private readonly IQueryService<GetUsers, IEnumerable<User>> getUsers;

        public UsersController(
            ICommandService<PromoteUserToRole> promoteUser,
            ICommandService<DemoteUserFromRole> demoteUser,
            IQueryService<GetUsers, IEnumerable<User>> getUsers)
        {
            if (promoteUser is null)
                throw new ArgumentNullException(nameof(promoteUser));

            if (demoteUser is null)
                throw new ArgumentNullException(nameof(demoteUser));

            if (getUsers is null)
                throw new ArgumentNullException(nameof(getUsers));

            this.promoteUser = promoteUser;
            this.demoteUser = demoteUser;
            this.getUsers = getUsers;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<User> users = null;
            GetUsers query = new GetUsers();

            string message = await this.CallServiceAsync(
                async () => users = await this.getUsers.ExecuteAsync(query));

            if (message != null)
            {
                return RedirectToAction(
                    nameof(HomeController.Index), "Home", new { area = "" })
                    .WithErrorMessage(message);
            }

            IEnumerable<UserListingViewModel> model =
                Mapper.Map<IEnumerable<UserListingViewModel>>(users);

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