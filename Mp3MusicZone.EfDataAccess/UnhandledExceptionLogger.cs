namespace Mp3MusicZone.EfDataAccess
{
    using Domain.Contracts;
    using Domain.Models;
    using System;

    public class UnhandledExceptionLogger : IExceptionLogger
    {
        private readonly IEfRepository<UnhandledExceptionEntry> unhandledExceptionRepository;
        private readonly IEfDbContextSaveChanges contextSaveChanges;
        private readonly IDateTimeProvider timeProvider;

        public UnhandledExceptionLogger(
            IEfRepository<UnhandledExceptionEntry> unhandledExceptionRepository,
            IEfDbContextSaveChanges contextSaveChanges,
            IDateTimeProvider timeProvider)
        {
            if (unhandledExceptionRepository is null)
                throw new ArgumentNullException(nameof(unhandledExceptionRepository));

            if (contextSaveChanges is null)
                throw new ArgumentNullException(nameof(contextSaveChanges));

            if (timeProvider is null)
                throw new ArgumentNullException(nameof(timeProvider));

            this.unhandledExceptionRepository = unhandledExceptionRepository;
            this.contextSaveChanges = contextSaveChanges;
            this.timeProvider = timeProvider;
        }

        public void Log(Exception exception, string additionalInfo = null)
        {
            string exceptionMessage = exception.Message;
            string exceptionType = exception.GetType().Name;

            if (exception.InnerException != null)
            {
                exceptionMessage += 
                    Environment.NewLine 
                    + "Inner Exception: " 
                    + exception.InnerException.Message;

                exceptionType += ", " + exception.InnerException.GetType().Name;
            }

            UnhandledExceptionEntry entry = new UnhandledExceptionEntry()
            {
                ExceptionMessage = exceptionMessage,
                TimeOfExecution = this.timeProvider.UtcNow,
                ExceptionType = exceptionType,
                StackTrace = exception.StackTrace,
                AdditionalInfo = additionalInfo
            };

            this.unhandledExceptionRepository.Add(entry);
            this.contextSaveChanges.SaveChanges();
        }
    }
}
