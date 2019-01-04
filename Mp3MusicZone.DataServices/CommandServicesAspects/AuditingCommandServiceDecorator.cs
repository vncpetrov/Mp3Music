namespace Mp3MusicZone.DomainServices.CommandServicesAspects
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class AuditingCommandServiceDecorator<TCommand> : ICommandService<TCommand>
    {
        private readonly IEfRepository<AuditEntry> auditingRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;
        private readonly IDateTimeProvider timeProvider;
        private readonly IUserContext userContext;
        private readonly ICommandService<TCommand> decoratee;

        public AuditingCommandServiceDecorator(
            IEfRepository<AuditEntry> auditingRepository,
            IEfDbContextSaveChanges contextSaveChanges,
            IDateTimeProvider timeProvider,
            IUserContext userContext,
            ICommandService<TCommand> decoratee)
        {
            if (auditingRepository is null)
                throw new ArgumentNullException(nameof(auditingRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            if (timeProvider is null)
                throw new ArgumentNullException(nameof(timeProvider));

            if (userContext is null)
                throw new ArgumentNullException(nameof(userContext));

            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.auditingRepository = auditingRepository;
            this.contextSaveChanges = contextSaveChanges;
            this.timeProvider = timeProvider;
            this.userContext = userContext;
            this.decoratee = decoratee;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            await this.decoratee.ExecuteAsync(command);
            this.AppendToAuditTrail(command);
        }

        private void AppendToAuditTrail(TCommand command)
        {
            AuditEntry entry = new AuditEntry()
            {
                UserId = this.userContext.GetCurrentUserId(),
                TimeOfExecution=this.timeProvider.UtcNow,
                Operation = command.GetType().Name,
                OperationData = JsonConvert.SerializeObject(command)
            };

            this.auditingRepository.Add(entry);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
