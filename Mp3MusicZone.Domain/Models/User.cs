namespace Mp3MusicZone.Domain
{
    using Models.Enums;
    using System;

    public class User
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }
        
        public DateTime Birthdate { get; set; }

        public GenreType Genre { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
