namespace Mp3MusicZone.Web.ViewModels.Manage
{
    using Mp3MusicZone.Common.ValidationAttributes;
    using Mp3MusicZone.Domain.Models.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class EditProfileViewModel
    {
        [Required]
        [MinLength(NameMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(NameMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(NameMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(NameMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [MinAge(16, ErrorMessage = MinAgeErrorMessage)]
        public DateTime Birthdate { get; set; }

        [Required]
        public GenreType Genre { get; set; }
    }
}
