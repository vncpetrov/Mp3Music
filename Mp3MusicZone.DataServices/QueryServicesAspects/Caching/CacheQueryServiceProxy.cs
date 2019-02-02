namespace Mp3MusicZone.DomainServices.QueryServicesAspects.Caching
{
    using Contracts;
    using Domain.Contracts;
    using System;
    using System.Threading.Tasks;

    public class CacheQueryServiceProxy<TQuery, TResult> : IQueryService<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : class
    {
        private readonly IQueryService<TQuery, TResult> queryService;
        private readonly ICacheManager cacheManager;
        private readonly IUserContext userContext;
        private readonly CacheOptions options;

        public CacheQueryServiceProxy(
            IQueryService<TQuery, TResult> queryService,
            ICacheManager cacheManager,
            IUserContext userContext,
            CacheOptions options)
        {
            if (cacheManager is null)
                throw new ArgumentNullException(nameof(cacheManager));

            if (userContext is null)
                throw new ArgumentNullException(nameof(userContext));

            if (queryService is null)
                throw new ArgumentNullException(nameof(queryService));

            this.queryService = queryService;
            this.cacheManager = cacheManager;
            this.userContext = userContext;
            this.options = options;
        }

        public async Task<TResult> ExecuteAsync(TQuery query)
        {
            string cacheKey = this.GetCacheKey(query);

            if (this.cacheManager.Exists(cacheKey))
            {
                return this.cacheManager.Get<TResult>(cacheKey);
            }

            TResult queryServiceResult = await this.queryService.ExecuteAsync(query);

            this.cacheManager.Add(
                cacheKey,
                queryServiceResult,
                this.options.AbsoluteDurationInSeconds);

            return queryServiceResult;
        }

        private string GetCacheKey(TQuery query)
        {
            string cacheKey = query.GetType().Name;

            if (this.options.VaryByUser)
            {
                cacheKey = query.GetType().Name + "-" + this.userContext.GetCurrentUserId();
            }

            return cacheKey;
        }
    }
}
