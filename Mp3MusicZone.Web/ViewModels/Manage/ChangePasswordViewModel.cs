namespace Mp3MusicZone.Web.Models.Manage 
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class ChangePasswordViewModel
    {
        [Required]
        [MinLength(PasswordMinLength, 
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(PasswordMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword),
           ErrorMessage = "New password and confirmation password do not match.")]
        [MinLength(PasswordMinLength,
            ErrorMessage = MinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength,
            ErrorMessage = MaxLengthErrorMessage)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
