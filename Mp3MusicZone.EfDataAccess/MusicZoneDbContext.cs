namespace Mp3MusicZone.EfDataAccess
{
    using Domain.Contracts;
    using EfDataAccess.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using System;

    public class MusicZoneDbContext : IdentityDbContext<UserEf>, IEfDbContextSaveChanges
    {
        public readonly string connectionString;

        public MusicZoneDbContext(DbContextOptions<MusicZoneDbContext> options)
             : base(options)
        {
            var extension = options.FindExtension<SqlServerOptionsExtension>();
            this.connectionString = extension.ConnectionString;
        }

        public MusicZoneDbContext(string connectionString)
            : base()
        {
            if (connectionString is null)
                throw new ArgumentNullException(nameof(connectionString));

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException(
                    "Value should not be empty.",
                    nameof(connectionString));

            this.connectionString = connectionString;
        }

        public DbSet<SongEf> Songs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEf>(entity
                =>
            { entity.ToTable(name: "Users"); });

            builder.Entity<IdentityRole>(entity
                =>
            { entity.ToTable(name: "Roles"); });

            builder.Entity<IdentityUserRole<string>>(
                entity => { entity.ToTable("UserRoles"); });

            builder.Entity<IdentityUserClaim<string>>(
                entity => { entity.ToTable("UserClaims"); });

            builder.Entity<IdentityUserLogin<string>>(
                entity => { entity.ToTable("UserLogins"); });

            builder.Entity<IdentityUserToken<string>>(
                entity => { entity.ToTable("UserToken"); });

            builder.Entity<IdentityRoleClaim<string>>(
                entity => { entity.ToTable("RoleClaim"); });

            builder.Entity<SongEf>(
                s => s.Property(p => p.PublishedOn)
                        .HasColumnType("date"));

            builder.Entity<SongEf>()
                .HasOne(s => s.Uploader)
                .WithMany(u => u.Songs)
                .HasForeignKey(s => s.UploaderId);
        }
    }
}
