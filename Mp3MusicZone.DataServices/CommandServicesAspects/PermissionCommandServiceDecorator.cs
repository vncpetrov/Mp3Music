namespace Mp3MusicZone.DomainServices.CommandServicesAspects
{
    using Contracts;
    using System;
    using System.Threading.Tasks;

    public class PermissionCommandServiceDecorator<TCommand> : ICommandService<TCommand>
    {
        private readonly ServicePermissionChecker<TCommand> permissionChecker;
        private readonly ICommandService<TCommand> decoratee;

        public PermissionCommandServiceDecorator(
            ServicePermissionChecker<TCommand> permissionChecker,
            ICommandService<TCommand> decoratee)
        {
            if (permissionChecker is null)
                throw new ArgumentNullException(nameof(permissionChecker));

            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.permissionChecker = permissionChecker;
            this.decoratee = decoratee;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            this.permissionChecker.CheckPermissionForCurrentUser();

            await this.decoratee.ExecuteAsync(command);
        }
    }
}
