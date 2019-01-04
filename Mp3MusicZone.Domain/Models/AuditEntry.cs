namespace Mp3MusicZone.Domain.Models
{
    using Models.Contracts;
    using System;

    public class AuditEntry : IDomainModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime TimeOfExecution { get; set; }

        public string Operation { get; set; }

        public string OperationData { get; set; }
    }
}
