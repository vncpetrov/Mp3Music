namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;

    public static class FormFileExtensions
    {
        public static byte[] ToByteArray(this IFormFile file)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                file.CopyTo(memory);

                return memory.ToArray();
            }
        }

        public static bool IsImage(this IFormFile file)
        {
            if (file is null)
            {
                return false;
            }

            return file.ContentType.Contains("image");
        }
    }
}
