namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Linq;

    public interface IEfRepository<T>
        where T : class
    {
        IQueryable<T> All();

        T GetById(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

    }
}
