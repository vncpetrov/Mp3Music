namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Text;

    public static class ControllerExtensions
    {
        public static string GetErrorsDescription(this Controller controller,
            IdentityResult result)
        {
            StringBuilder errors = new StringBuilder();

            foreach (var error in result.Errors)
            {
                errors.AppendLine(error.Description);    
            }

            return errors.ToString();
        }

        public static IActionResult RedirectToLocal(this Controller controller,
            string returnUrl)
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
