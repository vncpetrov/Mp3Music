namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public static class ControllerExtensions
    {
        public static void AddErrors(this Controller controller, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                controller.TempData.AddErrorMessage(error.Description);
            }
        }

        public static IActionResult RedirectToLocal(this Controller controller, string returnUrl)
        {
            if (controller.Url.IsLocalUrl(returnUrl))
            {
                return controller.Redirect(returnUrl);
            }
            else
            {
                return controller.RedirectToAction("Index", "Home");
            }
        }
    }
}
