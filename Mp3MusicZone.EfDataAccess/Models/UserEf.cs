namespace Mp3MusicZone.EfDataAccess.Models
{
    using AutoMapper;
    using Common.ValidationAttributes;
    using Domain.Models.Enums;
    using EfRepositories.Contracts;
    using MappingTables;
    using Microsoft.AspNetCore.Identity;
    using Mp3MusicZone.Common.Mappings;
    using Mp3MusicZone.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using static Mp3MusicZone.Common.Constants.ModelConstants;

    public class UserEf : IdentityUser,
        IEntityModel,
        IMapTo<User>, 
        IMapFrom<User>, 
        IHaveCustomMappings
    {
        [Required]
        [DataType(DataType.Date)]
        [MinAge(UserMinAge)]
        public DateTime Birthdate { get; set; }

        public GenreType Genre { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(StringMinLength)]
        [MaxLength(StringMaxLength)]
        public string LastName { get; set; }

        [MaxLength(ProfileImageMaxLength)]
        public byte[] ProfileImage { get; set; }

        public ICollection<SongEf> Songs { get; set; }
            = new HashSet<SongEf>();

        public ICollection<UserRole> Roles { get; set; } =
            new HashSet<UserRole>();

        public void Configure(Profile config)
        {
            config.CreateMap<UserEf, User>()
                .ForMember(d => d.Roles, opt => opt.MapFrom(
                    s => s.Roles.Select(
                        r => new
                        {
                            r.Role.Id,
                            r.Role.Name,
                            Permissions = r.Role.Permissions.Select(
                                p => new
                                {
                                    p.Permission.Id,
                                    p.Permission.Name
                                })
                        })));

            config.CreateMap<User, UserEf>()
                .ForMember(dest => dest.Songs, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());

            config.CreateMap<Role, UserRole>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));

            //config.CreateMap<User, UserRole>()
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        }
    }
}
