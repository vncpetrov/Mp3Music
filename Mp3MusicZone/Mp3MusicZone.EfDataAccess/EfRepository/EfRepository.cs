namespace Mp3MusicZone.EfDataAccess.EfRepository
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Mp3MusicZone.Domain.Contracts;
    using System;
    using System.Linq;

    public class EfRepository<TDomain, TEntity> : IEfRepository<TDomain>
        where TDomain : class
        where TEntity: class
    {
        private readonly MusicZoneDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public EfRepository(MusicZoneDbContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TDomain> All => this.dbSet.ProjectTo<TDomain>();

        public TDomain GetById(int id)
            => this.dbSet.Find(id);

        public void Add(TDomain entity)
        {
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

        public void Update(T entity)
        {
            EntityEntry entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Add(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
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
    }
}
