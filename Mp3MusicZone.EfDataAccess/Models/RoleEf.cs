namespace Mp3MusicZone.EfDataAccess.Models
{
    using AutoMapper;
    using EfRepositories.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Models.MappingTables;
    using Mp3MusicZone.Common.Mappings;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RoleEf : IdentityRole, IEntityModel, IHaveCustomMappings
    {
        public ICollection<RolePermission> Permissions { get; set; } =
            new HashSet<RolePermission>();

        public ICollection<UserRole> Users { get; set; } =
            new HashSet<UserRole>();

        public void Configure(Profile config)
        {
            config.CreateMap<Role, RoleEf>()
                .ForMember(dest => dest.NormalizedName, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Permissions, opt => opt.Condition(src => src.Permissions != null));

            config.CreateMap<Permission, RolePermission>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(p => p.Id));
            config.CreateMap<Role, RolePermission>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(r => r.Id));

            config.CreateMap<RoleEf, Role>();
        }
    }
}
