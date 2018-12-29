namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using AutoMapper;
    using AutoMapper.Configuration;
    using AutoMapper.QueryableExtensions;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Linq;

    public class RoleEfRepository : EfRepository<Role, RoleEf>
    {
        public RoleEfRepository(MusicZoneDbContext context)
            : base(context)
        {
        }

        public override IQueryable<Role> All(bool eagerLoading = false)
        {
            IQueryable<RoleEf> roles = this.dbSet.AsQueryable();

            if (eagerLoading)
            {
                return context.Roles
                    .Include(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .Include(r => r.Users)
                    .ThenInclude(ur => ur.User)
                    .ProjectTo<Role>();

                //return roles.Select(r => new
                //{
                //    r.Id,
                //    r.Name,
                //    Permissions = r.Permissions
                //        .Select(p => new
                //        {
                //            Id = p.Permission.Id,
                //            Name = p.Permission.Name
                //        })
                //})
                //.ProjectTo<Role>();
            }

            return roles.ProjectTo<Role>(
                new MapperConfiguration(
                    new MapperConfigurationExpression()));
        }
    }
}
