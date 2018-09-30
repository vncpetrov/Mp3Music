namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class UploadSongViewModel
    {
        [Required]
        [MinLength(StringMinLength, 
            ErrorMessage = "The Title must be at least 2 characters long.")]
        [MaxLength(StringMaxLength,
            ErrorMessage = "The Title must be at max 100 characters long.")]
        public string Title { get; set; }

        [Required]
        [MinLength(StringMinLength,
             ErrorMessage = "The Singer must be at least 2 characters long.")]
        [MaxLength(StringMaxLength,
             ErrorMessage = "The Singer must be at max 100 characters long.")]
        public string Singer { get; set; }

        [Required]
        [Range(SongMinYear, int.MaxValue,
            ErrorMessage = "The Released Year cannot be less than 1950.")]
        public int ReleasedYear { get; set; }

        [Required(ErrorMessage = "Please, choose a file!")]
        public IFormFile File { get; set; }
    }
}
