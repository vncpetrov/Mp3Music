namespace Mp3MusicZone.DomainServices.CommandServicesAspects
{
    using Contracts;
    using System;
    using System.Threading.Tasks;
    using System.Transactions;

    public class TransactionCommandServiceDecorator<TCommand> : ICommandService<TCommand>
    {
        private readonly ICommandService<TCommand> decoratee;

        public TransactionCommandServiceDecorator(ICommandService<TCommand> decoratee)
        {
            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.decoratee = decoratee;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await this.decoratee.ExecuteAsync(command);

                scope.Complete();
            }
        }
    }
}
