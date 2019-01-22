namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;

    public static class HttpContextExtensions
    {
        public static string GetRequestUrl(this HttpContext context)
        {
            string protocol = context.Request.Scheme;
            string host = context.Request.Host.Value;
            string path = context.Request.Path;
            string queryString = context.Request.QueryString.Value;

            string url = $"{protocol}://{host}{path}";

            if (!string.IsNullOrEmpty(queryString))
            {
                url += queryString;
            }

            return url;
        }
    }
}
