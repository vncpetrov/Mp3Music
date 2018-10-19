namespace Mp3MusicZone.Domain.Contracts
{
    using Models.Contracts;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEfRepository<TDomain>
        where TDomain : IDomainModel
    {
        IQueryable<TDomain> All { get; }

        Task<TDomain> GetByIdAsync(int id);

        void Add(TDomain item);

        void Update(TDomain item);

        void Delete(TDomain item);
    }
}
