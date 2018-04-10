namespace Mp3MusicZone.Domain
{
    using Microsoft.AspNetCore.Identity;
    using Models.Enums;
    using System;

    public class User : IdentityUser
    {
        public DateTime Birthdate { get; set; }

        public GenreType Genre { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
