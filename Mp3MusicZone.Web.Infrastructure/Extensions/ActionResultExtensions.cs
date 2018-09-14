namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    using static Mp3MusicZone.Common.Constants.WebConstants;

    public static class ActionResultExtensions
    {
        public static IActionResult WithSuccessMessage(this IActionResult action,
            string message)
        {
            return WithMessage(action, SuccessMessageType, message);
        }

        public static IActionResult WithErrorMessage(this IActionResult action, 
            string message)
        {
            return WithMessage(action, ErrorsMessageType, message);
        }

        public static IActionResult WithMessage(this IActionResult action, string type, string message)
        {
            return new WithMessageResult(action, type, message);
        }
    }
}
