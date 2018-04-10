namespace Mp3MusicZone.EfDataAccess
{
    using System;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Mp3MusicZone.Domain.Contracts;
    using Mp3MusicZone.EfDataAccess.Models;

    public class Mp3MusicZoneDbContext : IdentityDbContext<UserEf>, IEfDbContextSaveChanges
    {
        private readonly string connectionString;

        public Mp3MusicZoneDbContext(DbContextOptions<Mp3MusicZoneDbContext> options, string connectionString)
            : base(options)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Value should not be empty.",
                    nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
