namespace Mp3MusicZone.EfDataAccess.Models
{
    using Common.ValidationAttributes;
    using Domain.Models.Enums;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class UserEf : IdentityUser
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

        public ICollection<SongEf> Songs { get; set; } = new HashSet<SongEf>(); 
    }
}
