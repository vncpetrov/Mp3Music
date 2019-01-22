namespace Mp3MusicZone.Domain.Models
{
    using Contracts;
    using System;

    public class UnhandledExceptionEntry : IDomainModel
    {
        public string Id { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }
        
        public DateTime TimeOfExecution { get; set; }
         
        public string AdditionalInfo { get; set; }
         
        public string StackTrace { get; set; }
    }
}
