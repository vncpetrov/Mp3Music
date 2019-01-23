namespace Mp3MusicZone.Domain.Models
{
    using Contracts;
    using System;

    public class PerformanceEntry : IDomainModel
    {
        public string Id { get; set; }
        
        public DateTime TimeOfExecution { get; set; }
        
        public string Operation { get; set; }
        
        public TimeSpan Duration { get; set; }

        public string OperationData { get; set; }
    }
}
