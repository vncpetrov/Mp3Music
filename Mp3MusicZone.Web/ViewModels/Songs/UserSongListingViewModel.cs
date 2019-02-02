namespace Mp3MusicZone.Web.ViewModels.Songs
{
    using Common.Mappings;
    using Domain.Models;
    using System;

    public class UserSongListingViewModel : IMapFrom<Song>
    {
        public string PublishedOn { get; set; }

        public int Listenings { get; set; }

        public string Title { get; set; }

        public string Singer { get; set; }
    }
}
