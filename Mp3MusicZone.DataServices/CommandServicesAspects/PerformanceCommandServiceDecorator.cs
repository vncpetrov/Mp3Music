namespace Mp3MusicZone.DomainServices.CommandServicesAspects
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class PerformanceCommandServiceDecorator<TCommand> : ICommandService<TCommand>
    {
        private readonly IEfRepository<PerformanceEntry> performanceRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;
        private readonly IDateTimeProvider timeProvider; 
        private readonly ICommandService<TCommand> decoratee;

        public PerformanceCommandServiceDecorator(
            IEfRepository<PerformanceEntry> performanceRepository,
            IEfDbContextSaveChanges contextSaveChanges,
            IDateTimeProvider timeProvider, 
            ICommandService<TCommand> decoratee)
        {
            if (performanceRepository is null)
                throw new ArgumentNullException(nameof(performanceRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            if (timeProvider is null)
                throw new ArgumentNullException(nameof(timeProvider)); 

            if (decoratee is null)
                throw new ArgumentNullException(nameof(decoratee));

            this.performanceRepository = performanceRepository;
            this.contextSaveChanges = contextSaveChanges;
            this.timeProvider = timeProvider; 
            this.decoratee = decoratee;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            await this.decoratee.ExecuteAsync(command);
            stopwatch.Stop();

            this.AppendToPerformanceEntries(command, stopwatch.Elapsed);
        }

        private void AppendToPerformanceEntries(TCommand command, TimeSpan elapsed)
        {
            PerformanceEntry entry = new PerformanceEntry()
            { 
                TimeOfExecution = this.timeProvider.UtcNow,
                Operation = command.GetType().Name,
                OperationData = JsonConvert.SerializeObject(command),
                Duration = elapsed
            };

            this.performanceRepository.Add(entry);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
