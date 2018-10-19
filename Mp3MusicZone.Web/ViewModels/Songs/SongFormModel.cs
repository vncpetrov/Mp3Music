namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Mp3MusicZone.Common.Mappings;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class SongFormModel : IMapFrom<Song>, IHaveCustomMappings
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
        
        public IFormFile File { get; set; }

        public void Configure(Profile config)
        {
            config.CreateMap<Song, SongFormModel>()
                .ForMember(s => s.File, opt => opt.Ignore());
        }
    }
}
