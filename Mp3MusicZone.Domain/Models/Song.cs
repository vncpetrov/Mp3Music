namespace Mp3MusicZone.Domain.Models
{
    using Contracts;
    using System;

    public class Song : IDomainModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Singer { get; set; }
        
        public int ReleasedYear { get; set; }
        
        public DateTime PublishedOn { get; set; }

        public string FileExtension { get; set; }

        public string UploaderId { get; set; }
        public User Uploader { get; set; }
    }
}
