namespace Mp3MusicZone.Web.ViewModels.Manage 
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.ModelConstants;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }
        
        public string ProfileImageSource { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Genre { get; set; }

        public DateTime Birthdate { get; set; }
        
        public string StatusMessage { get; set; }

        public string Role { get; set; }
    }
}
