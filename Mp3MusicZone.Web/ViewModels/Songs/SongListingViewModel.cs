namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using AutoMapper;
    using Common.Mappings;
    using Domain.Models;
    using System;

    public class SongListingViewModel : IMapFrom<Song>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string PublishedOn { get; set; }

        public string FileExtension { get; set; }

        public string UploaderName { get; set; }

        public string HeadingText { get; set; }

        public void Configure(Profile config)
        {
            config.CreateMap<Song, SongListingViewModel>()
                .ForMember(s => s.UploaderName,
                    cfg => cfg.MapFrom(s => s.Uploader.UserName))
                .ForMember(s => s.HeadingText,
                    cfg => cfg.MapFrom(s => $"{s.Singer} - {s.Title}, {s.ReleasedYear}"))
                .ForMember(s => s.PublishedOn,
                    cfg => cfg.MapFrom(s => s.PublishedOn.ToString("dd MMM, yyyy")));
        }
    }
}
