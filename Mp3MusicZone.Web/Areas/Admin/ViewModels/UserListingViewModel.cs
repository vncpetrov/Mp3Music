namespace Mp3MusicZone.Web.Areas.Admin.ViewModels
{
    using AutoMapper;
    using Common.Mappings;
    using Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UserListingViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public bool EmailConfirmed { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public void Configure(Profile config)
        {
            config.CreateMap<User, UserListingViewModel>()
                .ForMember(d => d.Name,
                    cfg => cfg.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.Roles,
                    cfg => cfg.MapFrom(s => s.Roles.Select(r => r.Name).ToList()));
        }
    }
}
