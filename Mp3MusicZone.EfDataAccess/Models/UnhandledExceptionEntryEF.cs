namespace Mp3MusicZone.EfDataAccess.Models
{
    using EfRepositories.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UnhandledExceptionEntryEf : IEntityModel
    {
        public string Id { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        public string AdditionalInfo { get; set; }

        [Required]
        public DateTime TimeOfExecution { get; set; }

        [Required]
        public string StackTrace { get; set; }
    }
}
