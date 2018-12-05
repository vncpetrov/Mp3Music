namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using AutoMapper.QueryableExtensions;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Linq;

    public class UserEfRepository : EfRepository<User, UserEf>
    {
        public UserEfRepository(MusicZoneDbContext context)
            : base(context)
        {
        }

        public override IQueryable<User> All(bool eagerLoading = false)
        {
            IQueryable<UserEf> users = this.dbSet.AsQueryable();

            if (eagerLoading)
            {
                return users.Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Genre,
                    u.FirstName,
                    u.LastName,
                    u.Birthdate,
                    Roles = u.Roles.Select(r => new
                    {
                        r.Role.Id,
                        r.Role.Name,
                        Permissions = r.Role.Permissions
                            .Select(p => new
                            {
                                p.Permission.Id,
                                p.Permission.Name
                            })
                    })
                })
                .ProjectTo<User>();
            }

            return users.ProjectTo<User>();
        }
    }
}
