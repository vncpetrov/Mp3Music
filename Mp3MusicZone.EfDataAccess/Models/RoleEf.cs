namespace Mp3MusicZone.EfDataAccess.Models
{
    using AutoMapper;
    using Common.Mappings;
    using Domain.Models;
    using EfRepositories.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Models.MappingTables;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RoleEf : IdentityRole,
        IEntityModel,
        IMapTo<Role>, 
        IMapFrom<Role>, 
        IHaveCustomMappings
    {
        public ICollection<RolePermission> Permissions { get; set; } =
            new HashSet<RolePermission>();

        public ICollection<UserRole> Users { get; set; } =
            new HashSet<UserRole>();

        public void Configure(Profile config)
        {
            config.CreateMap<RoleEf, Role>()
                .ForMember(d => d.Permissions, cfg => cfg.MapFrom(
                    s => s.Permissions.Select(
                        p => new
                        {
                            p.Permission.Id,
                            p.Permission.Name
                        })));

            config.CreateMap<Role, RoleEf>()
                .ForMember(dest => dest.NormalizedName, opt => opt.Ignore())
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.Permissions, opt => opt.Condition(src => src.Permissions != null));

            config.CreateMap<Permission, RolePermission>()
                .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.Id));
            //config.CreateMap<Role, RolePermission>()
            //    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
