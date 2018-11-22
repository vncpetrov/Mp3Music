namespace Mp3MusicZone.EfDataAccess.Models
{
    using EfRepositories.Contracts;
    using Models.MappingTables;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Mp3MusicZone.Common.Mappings;
    using AutoMapper;
    using Mp3MusicZone.Domain.Models;

    public class PermissionEf : IEntityModel, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public ICollection<RolePermission> Roles { get; set; }
            = new HashSet<RolePermission>();

        public void Configure(Profile config)
        {
            config.CreateMap<PermissionEf, Permission>();

            config.CreateMap<Permission, PermissionEf>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
