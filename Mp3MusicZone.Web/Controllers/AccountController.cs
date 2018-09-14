namespace Mp3MusicZone.Web.Controllers
{
    using Auth.Contracts;
    using Domain.Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ViewModels.Account;
    using Mp3MusicZone.Web.Infrastructure.Filters;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Web.Infrastructure.Extensions;

    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly ISignInService signInService;
        private readonly IEmailSenderService emailSender;
        private readonly ILogger logger;

        public AccountController(
            IUserService userService,
            ISignInService signInService,
            IEmailSenderService emailSender,
            ILogger<AccountController> logger)
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
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(ManageController.Profile), "Manage");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Login(LoginViewModel model,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            UserEf user = await this.userService.FindByNameAsync(model.Username);
            if (user != null)
            {
                bool isEmailConfirmed = await this.userService
                .IsEmailConfirmedAsync(user);

                if (!isEmailConfirmed)
                {
                    return View(model)
                        .WithErrorMessage($@"Please, confirm your email address, to activate your account. If you cannot find your confirmation email <a href=""/account/resendemailconfirmation?email={user.Email}"">click here to resend it.</a>");
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await this.signInService
                .PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                logger.LogInformation("User logged in.");
                return this.RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form

            return View(model)
                .WithErrorMessage("Invalid login attempt.");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> Register(
            RegisterViewModel model,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            UserEf user = await this.userService.FindByEmailAsync(model.Email);

            if (user != null)
            {
                return View(model)
                    .WithErrorMessage($"E-mail address '{model.Email}' is already taken.");
            }

            user = new UserEf()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Genre = model.Genre,
                Birthdate = model.Birthdate
            };

            IdentityResult result =
                await this.userService.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");

                await this.SendEmailConfirmationToken(user);

                //await signInManager.SignInAsync(user, isPersistent: false);

                if (returnUrl is null)
                {
                    return View()
                        .WithSuccessMessage($"The registration is successfull. Please, verify your e-mail address {model.Email} before proceeding.");
                }

                return this.RedirectToLocal(returnUrl);
            }

            string errors = this.GetErrorsDescription(result);

            // If we got this far, something failed, redisplay form
            return View(model)
                .WithErrorMessage(errors);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            UserEf user = await this.userService.FindByEmailAsync(email);

            if (user == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage($"Unable to find user with email {email}.");
            }

            await this.SendEmailConfirmationToken(user);

            return View(nameof(this.Login))
                .WithSuccessMessage($"A verification link has been sent to email address {email}. Please verify your email.");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.signInService.SignOutAsync();
            logger.LogInformation("User logged out.");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            UserEf user = await this.userService.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            IdentityResult result = await this.userService.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                return RedirectToAction(nameof(this.Login))
                    .WithErrorMessage("Oops! Something went wrong. Please try again.");
            }

            return RedirectToAction(nameof(this.Login))
                .WithSuccessMessage("Thank you for verifying your email. You can now log in.");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            UserEf user = await this.userService.FindByEmailAsync(model.Email);

            if (user == null || !(await this.userService.IsEmailConfirmedAsync(user)))
            {
                return View()
                    .WithErrorMessage($"Invalid Email address.");
            }

            await SendForgotPasswordToken(user);

            return View()
                .WithSuccessMessage("Please check your email to reset your password.");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string email = null)
        {
            if (code == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage("A code must be supplied for password reset.");
            }

            if (this.userService.FindByEmailAsync(email) == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage("A valid email must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code, Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateModelState]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            UserEf user = await this.userService.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return this.RedirectToAction(nameof(HomeController.Index), "Home")
                    .WithErrorMessage("A valid email must be supplied for password reset.");
            }

            IdentityResult result = await this.userService
                .ResetPasswordAsync(user, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login))
                    .WithSuccessMessage("Your password has been reset successfully.");
            }

            string errors = this.GetErrorsDescription(result);
            return View()
                .WithErrorMessage(errors);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task SendEmailConfirmationToken(UserEf user)
        {
            string code = await this.userService.GenerateEmailConfirmationTokenAsync(user);
            string callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

            await emailSender.SendEmailConfirmationAsync(user.Email, callbackUrl);
        }

        private async Task SendForgotPasswordToken(UserEf user)
        {
            string code = await this.userService.GeneratePasswordResetTokenAsync(user);
            string callbackUrl =
                Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

            callbackUrl += $"&email={user.Email}";

            await emailSender.SendEmailAsync(user.Email, "Reset Password",
               $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        }


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public IActionResult ExternalLogin(string provider, string returnUrl = null)
        //{
        //    // Request a redirect to the external login provider.
        //    var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        //    var properties = this.signInService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return Challenge(properties, provider);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, 
        //    string remoteError = null)
        //{
        //    if (remoteError != null)
        //    {
        //        ErrorMessage = $"Error from external provider: {remoteError}";
        //        return RedirectToAction(nameof(Login));
        //    }
        //    var info = await this.signInService.GetExternalLoginInfoAsync();
        //    if (info == null)
        //    {
        //        return RedirectToAction(nameof(Login));
        //    }

        //    // Sign in the user with this external login provider if the user already has a login.
        //    var result = await this.signInService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        //    if (result.Succeeded)
        //    {
        //        logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
        //        return this.RedirectToLocal(returnUrl);
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return RedirectToAction(nameof(Lockout));
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then ask the user to create an account.
        //        ViewData["ReturnUrl"] = returnUrl;
        //        ViewData["LoginProvider"] = info.LoginProvider;
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        //{
        //    string errors = string.Empty;

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await this.signInService.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            throw new ApplicationException("Error loading external login information during confirmation.");
        //        }

        //        UserEf user = new UserEf { UserName = model.Email, Email = model.Email };
        //        IdentityResult result = await this.userService.CreateAsync(user);

        //        if (result.Succeeded)
        //        {
        //            result = await this.userService.AddLoginAsync(user, info);
        //            if (result.Succeeded)
        //            {
        //                await this.signInService.SignInAsync(user, isPersistent: false);
        //                logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
        //                return this.RedirectToLocal(returnUrl);
        //            }
        //        }

        //        errors = this.GetErrorsDescription(result);
        //    }

        //    ViewData["ReturnUrl"] = returnUrl;

        //    return View(nameof(ExternalLogin), model)
        //        .WithErrorMessage(errors);
        //}


        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Lockout()
        //{
        //    return View();
        //}

    }
}
