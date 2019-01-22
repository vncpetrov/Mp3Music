namespace Mp3MusicZone.Web.Infrastructure.Filters
{
    using Domain.Exceptions;
    using Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;

    public class CommonExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception.GetType().Name)
            {
                case nameof(NotAuthorizedException):
                    context.Result = new RedirectToActionResult(
                        "Index", "Home", new { area = "" })
                        .WithErrorMessage("You do not have permissions to perform this action.");
                    break;

                case nameof(InvalidOperationException):
                    IActionResult result = new RedirectToActionResult(
                        "Index", "Home", new { area = "" });

                    string referer = context.HttpContext.Request.Headers["Referer"];

                    if (referer != null)
                    {
                        result = new RedirectResult(referer);
                    }

                    context.Result =  result.WithErrorMessage(
                            "We're sorry, something went wrong. Please try again later.");

                    break;
            }
        }
    }
}
