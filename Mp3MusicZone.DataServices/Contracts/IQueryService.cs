namespace Mp3MusicZone.DomainServices.Contracts
{
    using System;
    using System.Threading.Tasks;

    public interface IQueryService<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query);
    }
}
