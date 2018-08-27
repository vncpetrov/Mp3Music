namespace Mp3MusicZone.Web.Models.Manage 
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.ModelConstants;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string StatusMessage { get; set; }

        [MaxLength(ProfileImageMaxLength)]
        public byte[] ProfileImage { get; set; }

    }
}
