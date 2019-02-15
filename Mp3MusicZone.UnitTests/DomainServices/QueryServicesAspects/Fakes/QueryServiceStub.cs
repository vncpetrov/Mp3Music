namespace Mp3MusicZone.UnitTests.DomainServices.QueryServicesAspects.Fakes
{
    using Mp3MusicZone.DomainServices.Contracts;
    using System;
    using System.Threading.Tasks;

    public class QueryServiceStub : IQueryService<QueryStub, object>
    {
        public async Task<object> ExecuteAsync(QueryStub query)
        {
            return await Task.FromResult<object>(null);
        }
    }
}
