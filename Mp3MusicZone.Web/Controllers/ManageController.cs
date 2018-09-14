namespace Mp3MusicZone.Web.Controllers
{
    using Auth.Contracts;
    using Domain.Contracts;
    using Domain.Models.Enums;
    using EfDataAccess.Models;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.Manage;
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using ViewModels.Manage;
    using Web.Infrastructure.Extensions;

    using static Common.Constants.ModelConstants;
    using static Common.Constants.WebConstants;


    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly IUserService userService;
        private readonly ISignInService signInService;
        private readonly IEmailSenderService emailSender;
        private readonly ILogger logger;
        private readonly UrlEncoder urlEncoder;

        private const string AuthenticatorUriFormat =
            "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
          IUserService userService,
          ISignInService signInService,
          IEmailSenderService emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            if (userService is null)
                throw new ArgumentNullException(nameof(userService));

            if (signInService is null)
                throw new ArgumentNullException(nameof(signInService));

            if (emailSender is null)
                throw new ArgumentNullException(nameof(emailSender));

            this.userService = userService;
            this.signInService = signInService;
            this.emailSender = emailSender;
            this.logger = logger;
            this.urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            UserEf user = await this.userService.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            EditProfileViewModel model = new EditProfileViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthdate = user.Birthdate,
                Genre = user.Genre
            };

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            UserEf user = await this.userService.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Genre = model.Genre;
            user.Birthdate = model.Birthdate;

            IdentityResult updateUserResult = await this.userService.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                string errors = this.GetErrorsDescription(updateUserResult);

                return View(model)
                    .WithErrorMessage(errors);
            }

            return RedirectToAction(nameof(Profile))
                .WithSuccessMessage("Your profile has been successfully updated.");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            UserEf user = await this.userService.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            string messageType = string.Empty;
            string messageText = string.Empty;

            if (!file.IsImage() || file.Length > ProfileImageMaxLength)
            {
                messageType = ErrorsMessageType;
                messageText = $"Your submission should be a image file and no more than 5 MBs in size!";

                //this.TempData.AddErrorMessage($"Your submission should be a image file and no more than 5 MBs in size!");
            }
            else
            {
                user.ProfileImage = file.ToByteArray();

                IdentityResult result = await this.userService.UpdateAsync(user);

                if (result.Succeeded)
                {
                    messageType = SuccessMessageType;
                    messageText = "Profile Picture uploaded successfully.";

                    //this.TempData.AddSuccessMessage("Profile Picture uploaded successfully.");
                }
                else
                {
                    messageType = ErrorsMessageType;
                    messageText = "We're sorry, something went wrong. Please try again later.";

                    //this.TempData.AddErrorMessage("We're sorry, something went wrong. Please try again later.");
                }
            }

            return RedirectToAction(nameof(Profile))
                .WithMessage(messageType, messageText);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            UserEf user = await this.userService.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            string profileImageSource = user.ProfileImage == null ?
            "../images/NoImage.png"
            : string.Format("data:{0};base64,{1}",
                "image/*",
                 Convert.ToBase64String(user.ProfileImage));

            string mostSignificantRole = GetCurrentUserRole();
            string genre = user.Genre == 0 ? "-" : user.Genre.ToString();

            ProfileViewModel model = new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                StatusMessage = StatusMessage,
                ProfileImageSource = profileImageSource,
                Role = mostSignificantRole,
                Birthdate = user.Birthdate,
                Genre = genre
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationEmail(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.userService.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            var code = await this.userService.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            var email = user.Email;
            await emailSender.SendEmailConfirmationAsync(email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            UserEf user = await this.userService.GetUserAsync(User);
            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            ChangePasswordViewModel model = new ChangePasswordViewModel
            {
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserEf user = await this.userService.GetUserAsync(User);

            if (user is null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
            }

            IdentityResult changePasswordResult = await this.userService
                    .ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                string errors = this.GetErrorsDescription(changePasswordResult);

                return View(model)
                    .WithErrorMessage(errors);
            }

            await signInService.SignInAsync(user, isPersistent: false);
            logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            //this.TempData.AddSuccessMessage("Your password has been successfully changed.");

            return RedirectToAction(nameof(Profile))
                .WithSuccessMessage("Your password has been successfully changed.");
        }

        #region Helpers

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                urlEncoder.Encode("Mp3MusicZone.Web"),
                urlEncoder.Encode(email),
                unformattedKey);
        }

        private string GetCurrentUserRole()
        {
            string mostSignificantRole = string.Empty;

            if (User.IsInRole(Role.Administrator.ToString()))
            {
                mostSignificantRole = Role.Administrator.ToString();
            }
            else if (this.User.IsInRole(Role.Uploader.ToString()))
            {
                mostSignificantRole = Role.Uploader.ToString();
            }

            return mostSignificantRole;
        }

        //private async Task LoadSharedKeyAndQrCodeUriAsync(UserEf user, EnableAuthenticatorViewModel model)
        //{
        //    var unformattedKey = await this.userService.GetAuthenticatorKeyAsync(user);
        //    if (string.IsNullOrEmpty(unformattedKey))
        //    {
        //        await this.userService.ResetAuthenticatorKeyAsync(user);
        //        unformattedKey = await this.userService.GetAuthenticatorKeyAsync(user);
        //    }

        //    model.SharedKey = FormatKey(unformattedKey);
        //    model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        //}
        #endregion

        //External login
       //[HttpGet]
       // public async Task<IActionResult> ExternalLogins()
       // {
       //     var user = await this.userService.GetUserAsync(User);
       //     if (user == null)
       //     {
       //         throw new ApplicationException($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
       //     }

       //     var model = new ExternalLoginsViewModel { CurrentLogins = await this.userService.GetLoginsAsync(user) };
       //     model.OtherLogins = (await signInService.GetExternalAuthenticationSchemesAsync())
       //         .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
       //         .ToList();
       //     model.ShowRemoveButton = await this.userService.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
       //     model.StatusMessage = StatusMessage;

       //     return View(model);
       // }

        //[HttpPost]
        //public async Task<IActionResult> LinkLogin(string provider)
        //{
        //    Clear the existing external cookie to ensure a clean login process
        //   await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //    Request a redirect to the external login provider to link a login for the current user

        //   var redirectUrl = Url.Action(nameof(LinkLoginCallback));
        //    var properties = signInService.ConfigureExternalAuthenticationProperties(provider, redirectUrl, this.userService.GetUserId(User));
        //    return new ChallengeResult(provider, properties);
        //}

        //[HttpGet]
        //public async Task<IActionResult> LinkLoginCallback()
        //{
        //    var user = await this.userService.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
        //    }

        //    var info = await signInService.GetExternalLoginInfoAsync(user.Id);
        //    if (info == null)
        //    {
        //        throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
        //    }

        //    var result = await this.userService.AddLoginAsync(user, info);
        //    if (!result.Succeeded)
        //    {
        //        throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
        //    }

        //    Clear the existing external cookie to ensure a clean login process
        //   await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //    StatusMessage = "The external login was added.";
        //    return RedirectToAction(nameof(ExternalLogins));
        //}

        //[HttpPost]
        //public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        //{
        //    var user = await this.userService.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{this.userService.GetUserId(User)}'.");
        //    }

        //    var result = await this.userService.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
        //    if (!result.Succeeded)
        //    {
        //        throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
        //    }

        //    await signInService.SignInAsync(user, isPersistent: false);
        //    StatusMessage = "The external login was removed.";
        //    return RedirectToAction(nameof(ExternalLogins));
        //}
    }
}
