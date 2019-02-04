namespace Mp3MusicZone.DomainServices.CommandServicesAspects
{
    using Domain.Attributes;
    using Domain.Contracts;
    using DomainServices.Contracts;
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    public class InvalidateCacheCommandServiceDecorator<TCommand> : ICommandService<TCommand>
    {
        private static readonly string[] keys;

        private readonly ICacheManager cacheManager;
        private readonly ICommandService<TCommand> decoratee;

        static InvalidateCacheCommandServiceDecorator()
        {
            InvalidateCacheForAttribute invalidateCacheAttr =
                typeof(TCommand).GetCustomAttribute<InvalidateCacheForAttribute>();

            if (invalidateCacheAttr is null)
            {
                throw new InvalidOperationException($"The type {typeof(TCommand).Name} is not marked with the [{typeof(InvalidateCacheForAttribute).Name}]. Please define the cached keys that should be invalidated.");
            }

            keys = invalidateCacheAttr.Keys;
        }

        public InvalidateCacheCommandServiceDecorator(
            ICacheManager cacheManager,
            ICommandService<TCommand> decoratee)
        {
            if (cacheManager is null)
                throw new ArgumentNullException(nameof(cacheManager));

            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.cacheManager = cacheManager;
            this.decoratee = decoratee;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            await this.decoratee.ExecuteAsync(command);
            
            foreach (var key in keys)
            {
                this.cacheManager.Remove(key);
            }
        }
    }
}
