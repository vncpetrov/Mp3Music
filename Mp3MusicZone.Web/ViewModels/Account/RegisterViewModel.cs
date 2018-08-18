namespace Mp3MusicZone.Web.Models.Account
{
    using AutoMapper;
    using Common.Mappings;
    using Common.ValidationAttributes;
    using Domain.Models.Enums;
    using EfDataAccess.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class RegisterViewModel : IMapTo<UserEf>, IHaveCustomMappings
    {
        [Required]
        [MinLength(UsernameMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(UsernameMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        public string Username { get; set; }

        [Required]
        [MinLength(PasswordMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password),
            ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

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

        public void Configure(Profile config)
        {
            config.CreateMap<RegisterViewModel, UserEf>()
                .ForMember(x => x.AccessFailedCount, opt => opt.Ignore())
                .ForMember(x => x.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(x => x.EmailConfirmed, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.LockoutEnabled, opt => opt.Ignore())
                .ForMember(x => x.LockoutEnd, opt => opt.Ignore())
                .ForMember(x => x.NormalizedEmail, opt => opt.Ignore())
                .ForMember(x => x.NormalizedUserName, opt => opt.Ignore())
                .ForMember(x => x.PasswordHash, opt => opt.Ignore())
                .ForMember(x => x.PhoneNumber, opt => opt.Ignore())
                .ForMember(x => x.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(x => x.SecurityStamp, opt => opt.Ignore())
                .ForMember(x => x.TwoFactorEnabled, opt => opt.Ignore());
        }
    }
}
