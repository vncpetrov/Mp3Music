namespace Mp3MusicZone.Domain.Models
{
    using Models.Contracts;
    using Models.Enums;
    using System;
    using System.Collections.Generic;

    public class User : IDomainModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public DateTime Birthdate { get; set; }

        public GenreType Genre { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] ProfileImage { get; set; }

        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}