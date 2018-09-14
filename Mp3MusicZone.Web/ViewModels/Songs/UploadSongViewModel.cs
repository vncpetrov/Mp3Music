namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class UploadSongViewModel
    {
        [Required]
        [MinLength(NameMinLength, ErrorMessage = "The Title must be at least 2 characters long.")]
        [MaxLength(NameMaxLength, ErrorMessage = "The Title must be at max 100 characters long.")]
        public string Title { get; set; }

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Singer { get; set; }

        [Required(ErrorMessage = "Please, choose a file!")]
        public IFormFile Song { get; set; }
    }
}
