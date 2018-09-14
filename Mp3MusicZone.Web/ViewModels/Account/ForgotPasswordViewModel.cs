namespace Mp3MusicZone.Web.ViewModels.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
