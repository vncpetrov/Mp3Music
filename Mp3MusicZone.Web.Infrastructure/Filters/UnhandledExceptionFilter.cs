namespace Mp3MusicZone.Web.Infrastructure.Filters
{
    using Domain.Contracts;
    using Domain.Models;
    using EfDataAccess;
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System;

    public class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly IExceptionLogger logger;

        public UnhandledExceptionFilter(IExceptionLogger logger)
        {
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));

            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Result is null)
            {
                this.LogException(context);

                IActionResult result = new RedirectToActionResult(
                        "Index", "Home", new { area = "" });

                string referer = context.HttpContext.Request.Headers["Referer"];

                if (referer != null)
                {
                    result = new RedirectResult(referer);
                }

                context.Result = result.WithErrorMessage(
                        "We're sorry, something went wrong. Please try again later.");
            }
        }

        private void LogException(ExceptionContext context)
        {
            string url = context.HttpContext.GetRequestUrl();
            string method = context.HttpContext.Request.Method;
            string additionalInfo = $"url: {url} | method: {method}";
            
            this.logger.Log(context.Exception, additionalInfo);
        }
    }
}
