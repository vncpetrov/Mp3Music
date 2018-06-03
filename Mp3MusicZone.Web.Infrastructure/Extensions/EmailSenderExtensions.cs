namespace Mp3MusicZone.Web.Infrastructure.Extensions
{
    using Domain.Contracts;
    using System;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSenderService emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
