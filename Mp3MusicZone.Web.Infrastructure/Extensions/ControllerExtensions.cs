namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Mp3MusicZone.Domain.Exceptions;
    using System;
    using System.Text;
    using System.Threading.Tasks;

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

        public static async Task<string> CallServiceAsync(this Controller controller, Func<Task> func)
        {
            string message = null;

            try
            {
                await func();
            }
            catch (InvalidOperationException ex)
            {
                message = ex.Message;
            }
            catch (NotAuthorizedException ex)
            {
                message = "You do not have permissions to perform this action.";
            }
            catch (Exception ex)
            {
                message = "We're sorry, something went wrong. Please try again later.";
            }

            return message;
        }

        public static string CallService(this Controller controller, Action action)
        {
            string message = null;

            try
            {
                action();
            }
            catch (InvalidOperationException ex)
            {
                message = ex.Message;
            }
            catch (NotAuthorizedException ex)
            {
                message = "You do not have permissions to perform this action.";
            }
            catch (Exception)
            {
                message = "We're sorry, something went wrong. Please try again later.";
            }

            return message;
        }
    }
}
