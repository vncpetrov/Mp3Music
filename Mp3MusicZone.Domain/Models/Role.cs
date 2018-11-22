namespace Mp3MusicZone.Domain.Models
{
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class Role : IDomainModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
    }
}
