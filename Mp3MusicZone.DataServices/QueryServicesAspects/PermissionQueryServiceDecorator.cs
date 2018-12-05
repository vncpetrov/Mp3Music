namespace Mp3MusicZone.DomainServices.QueryServicesAspects
{
    using Contracts;
    using System;
    using System.Threading.Tasks;

    public class PermissionQueryServiceDecorator<TQuery, TResult>
        : IQueryService<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly ServicePermissionChecker<TQuery> permissionChecker;
        private readonly IQueryService<TQuery, TResult> decoratee;

        public PermissionQueryServiceDecorator(
            ServicePermissionChecker<TQuery> permissionChecker,
            IQueryService<TQuery, TResult> decoratee)
        {
            if (permissionChecker is null)
                throw new ArgumentNullException(nameof(permissionChecker));

            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.permissionChecker = permissionChecker;
            this.decoratee = decoratee;
        }

        public async Task<TResult> ExecuteAsync(TQuery query)
        {
            this.permissionChecker.CheckPermissionForCurrentUser();

            return await this.decoratee.ExecuteAsync(query);
        }
    }
}
