namespace Mp3MusicZone.Domain.Models
{
    using Contracts;
    using System;

    public class Permission : IDomainModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}