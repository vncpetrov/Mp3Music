namespace Mp3MusicZone.EfDataAccess.Models
{
    using EfRepositories.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PerformanceEntryEf : IEntityModel
    {
        public string Id { get; set; }

        [Required]
        public DateTime TimeOfExecution { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        public string OperationData { get; set; }
    }
}