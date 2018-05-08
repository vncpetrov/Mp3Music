namespace Mp3MusicZone.Domain.Contracts
{
    using System;
    using System.Linq;

    public interface IEfRepository<TDomain>
        where TDomain : class
    {
        IQueryable<TDomain> All { get; }

        TDomain GetById(int id);

        void Add(TDomain item);

        void Update(TDomain item);

        void Delete(TDomain item); 
    }
}
