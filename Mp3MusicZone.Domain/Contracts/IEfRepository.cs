namespace Mp3MusicZone.Domain.Contracts
{
    using Mp3MusicZone.Domain.Models.Contracts;
    using System;
    using System.Linq;

    public interface IEfRepository<TDomain>
        where TDomain : IDomainModel
    {
        IQueryable<TDomain> All { get; }

        TDomain GetById(int id);

        void Add(TDomain item);

        void Update(TDomain item);

        void Delete(TDomain item);
    }
}
