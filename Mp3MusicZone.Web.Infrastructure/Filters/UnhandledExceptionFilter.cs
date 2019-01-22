namespace Mp3MusicZone.Web.Infrastructure.Filters
{
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.Domain.Models;
    using Mp3MusicZone.EfDataAccess;
    using System;

    public class UnhandledExceptionFilter : IExceptionFilter
    {
        private readonly IEfRepository<UnhandledExceptionEntry> unhandledExceptionRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;
        private readonly IDateTimeProvider timeProvider;

        public UnhandledExceptionFilter(
            IEfRepository<UnhandledExceptionEntry> unhandledExceptionRepository,
            MusicZoneDbContext efDbContext,
            IDateTimeProvider timeProvider)
        {
            if (unhandledExceptionRepository is null)
                throw new ArgumentNullException(nameof(unhandledExceptionRepository));

            if (efDbContext is null)
                throw new ArgumentNullException(nameof(efDbContext));

            if (timeProvider is null)
                throw new ArgumentNullException(nameof(timeProvider));

            this.unhandledExceptionRepository = unhandledExceptionRepository;
            this.contextSaveChanges = efDbContext;
            this.timeProvider = timeProvider;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Result is null)
            {
                // log exception
                this.LogUnhandledException(context);

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

        private void LogUnhandledException(ExceptionContext context)
        {
            string url = this.GetUrl(context.HttpContext);
            string exceptionMessage =
                context.Exception.Message ?? context.Exception.InnerException.Message;

            UnhandledExceptionEntry entry = new UnhandledExceptionEntry()
            {
                ExceptionMessage = exceptionMessage,
                TimeOfExecution = this.timeProvider.UtcNow,
                ExceptionType = context.Exception.GetType().Name,
                StackTrace = context.Exception.StackTrace,
                Url = url
            };

            this.unhandledExceptionRepository.Add(entry);
            this.contextSaveChanges.SaveChanges();
        }

        private string GetUrl(HttpContext httpContext)
        {
            string protocol = httpContext.Request.Scheme;
            string host = httpContext.Request.Host.Value;
            string path = httpContext.Request.Path;

            return $"{protocol}://{host}{path}";
        }
    }
}
