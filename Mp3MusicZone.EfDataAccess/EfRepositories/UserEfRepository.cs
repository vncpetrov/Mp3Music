namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using AutoMapper;
    using AutoMapper.Configuration;
    using AutoMapper.QueryableExtensions;
    using Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                return context.Users
                    .Include(u => u.Roles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.Permissions)
                    .ThenInclude(rp => rp.Permission)
                    .ProjectTo<User>();
            }

            return users.ProjectTo<User>(
                new MapperConfiguration(
                    new MapperConfigurationExpression()));
        }

        public override void Update(User item)
        {
            UserEf entity = this.dbSet.Find(item.Id);
            this.context.Entry(entity).Collection(u => u.Roles).Load();

            entity = Mapper.Map<User, UserEf>(item, entity);

            EntityEntry entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Add(entity);
            }

            entry.State = EntityState.Modified;
        }
    }
}
