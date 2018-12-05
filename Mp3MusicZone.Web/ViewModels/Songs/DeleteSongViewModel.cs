namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using AutoMapper;
    using Common.Mappings;
    using Domain.Models;
    using System;

    public class DeleteSongViewModel : IMapFrom<Song>, IHaveCustomMappings
    {
        public string SongId { get; set; }

        public string SongSummaryText { get; set; }

        public void Configure(Profile config)
        {
            config.CreateMap<Song, DeleteSongViewModel>()
                .ForMember(
                    dest => dest.SongSummaryText,
                    opt => opt.MapFrom(src => 
                        $"{src.Singer} - {src.Title}, {src.ReleasedYear}"));
        }
    }
}
