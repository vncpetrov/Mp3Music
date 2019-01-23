namespace Mp3MusicZone.DomainServices.QueryServicesAspects
{
    using Contracts;
    using Domain.Contracts;
    using Domain.Models;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class PerformanceQueryServiceDecorator<TQuery, TResult>
        : IQueryService<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IEfRepository<PerformanceEntry> performanceRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;
        private readonly IDateTimeProvider timeProvider;
        private readonly IQueryService<TQuery, TResult> decoratee;

        public PerformanceQueryServiceDecorator(
            IEfRepository<PerformanceEntry> performanceRepository,
            IEfDbContextSaveChanges contextSaveChanges,
            IDateTimeProvider timeProvider,
            IQueryService<TQuery, TResult> decoratee)
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

        public async Task<TResult> ExecuteAsync(TQuery query)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            TResult queryResult = await this.decoratee.ExecuteAsync(query);
            stopwatch.Stop();

            this.AppendToPerformanceEntries(query, stopwatch.Elapsed);
            return queryResult;
        }

        private void AppendToPerformanceEntries(TQuery command, TimeSpan elapsed)
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
