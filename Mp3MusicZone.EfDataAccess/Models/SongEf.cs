﻿namespace Mp3MusicZone.EfDataAccess.Models
{
    using EfRepositories.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Common.Constants.ModelConstants;

    public class SongEf : IEntityModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string Singer { get; set; }

        [Required]
        [Range(SongMinYear, int.MaxValue)]
        public int ReleasedYear { get; set; }

        [Required]
        public DateTime PublishedOn { get; set; }

        [Required]
        public string FileExtension { get; set; }

        public bool IsApproved { get; set; }

        public int Listenings { get; set; }

        public string UploaderId { get; set; }
        public UserEf Uploader { get; set; }
    }
}
