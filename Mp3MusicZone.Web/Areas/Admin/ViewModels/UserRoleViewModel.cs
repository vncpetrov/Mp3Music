namespace Mp3MusicZone.Web.Areas.Admin.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserRoleViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
