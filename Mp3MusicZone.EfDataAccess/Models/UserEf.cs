namespace Mp3MusicZone.EfDataAccess.Models
{
    using Common.ValidationAttributes;
    using Domain.Models.Enums;
    using EfRepositories.Contracts;
    using MappingTables;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class UserEf : IdentityUser, IEntityModel
    {
        [Required]
        [DataType(DataType.Date)]
        [MinAge(UserMinAge)]
        public DateTime Birthdate { get; set; }

        public GenreType Genre { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string LastName { get; set; }

        [MaxLength(ProfileImageMaxLength)]
        public byte[] ProfileImage { get; set; }

        public ICollection<SongEf> Songs { get; set; } 
            = new HashSet<SongEf>();

        public ICollection<UserRole> Roles { get; set; } =
            new HashSet<UserRole>();

        //public ICollection<IdentityUserRole<string>> Roles { get; set; } =
        //    new HashSet<IdentityUserRole<string>>();
    }
}
