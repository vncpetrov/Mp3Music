namespace Mp3MusicZone.EfDataAccess.EfRepositories
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Domain.Contracts;
    using Domain.Models.Contracts;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using System.Linq;

    public abstract class EfRepository<TDomain, TEntity> : IEfRepository<TDomain>
        where TDomain : class, IDomainModel, new()
        where TEntity : class, IEntityModel, new()
    {
        private MusicZoneDbContext context;
        private DbSet<TEntity> dbSet;

        protected EfRepository(MusicZoneDbContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TDomain> All => this.dbSet.ProjectTo<TDomain>();

        public void Add(TDomain item)
        {
            TEntity entity = Mapper.Map<TEntity>(item);
            EntityEntry entry = this.context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.context.Add(entity);
            }
        }

        public void Delete(TDomain item)
        {
            TEntity entity = this.dbSet.Find(item.Id);
            EntityEntry entry = this.context.Entry(entity);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.dbSet.Attach(entity);
                this.dbSet.Remove(entity);
            }
        }

        public TDomain GetById(int id)
        {
            TEntity entity = this.dbSet.Find(id);
            TDomain domainModel = Mapper.Map<TDomain>(entity);

            return domainModel;
        }

        public void Update(TDomain item)
        {
            TEntity entity = this.dbSet.Find(item.Id);
            entity = Mapper.Map<TDomain, TEntity>(item, entity);

            EntityEntry entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Add(entity);
            }

            entry.State = EntityState.Modified;
        }
    }
}
