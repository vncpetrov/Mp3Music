namespace Mp3MusicZone.EfDataAccess.Models
{
    using EfRepositories.Contracts;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AuditEntryEf : IEntityModel
    {
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public UserEf User { get; set; }

        [Required]
        public DateTime TimeOfExecution { get; set; }

        [Required]
        public string Operation { get; set; }

        public string OperationData { get; set; }
    }
}
