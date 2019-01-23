namespace Mp3MusicZone.EfDataAccess
{
    using Domain.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
    using Models;
    using Models.MappingTables;
    using System;

    public class MusicZoneDbContext :
        IdentityDbContext
        <
            UserEf,                    // TUser
            RoleEf,                    // TRole
            string,                    // TKey
            IdentityUserClaim<string>, // TUserClaim
            UserRole,                  // TUserRole,
            IdentityUserLogin<string>, // TUserLogin
            IdentityRoleClaim<string>, // TRoleClaim
            IdentityUserToken<string>  // TUserToken
        >,
        IEfDbContextSaveChanges
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
        public DbSet<PermissionEf> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuditEntryEf> AuditEntries { get; set; }
        public DbSet<UnhandledExceptionEntryEf> UnhandledExceptionEntries { get; set; }
        public DbSet<PerformanceEntryEf> PerformanceEntries { get; set; }
        
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

            builder.Entity<UserEf>(
                entity => entity.ToTable(name: "Users"));

            builder.Entity<RoleEf>(
                entity => entity.ToTable(name: "Roles"));

            //builder.Entity<IdentityUserRole<string>>(
            //    entity => entity.ToTable(name: "UserRoles"));

            //builder.Entity<UserRole>()
            //    .HasKey(ur => new { ur.UserId, ur.RoleId });

            //builder.Entity<UserEf>()
            //    .HasMany(u => u.Roles)
            //    .WithOne(ur => ur.User)
            //    .HasForeignKey(ur => ur.UserId);

            //builder.Entity<RoleEf>()
            //    .HasMany(u => u.Users)
            //    .WithOne(ur => ur.Role)
            //    .HasForeignKey(ur => ur.RoleId);

            builder.Entity<UserRole>()
                .ToTable(name: "UserRoles");

            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(ur => ur.RoleId);

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(r => r.Roles)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserRole>(
                entity => entity.ToTable("UserRoles"));

            builder.Entity<IdentityUserClaim<string>>(
                entity => entity.ToTable("UserClaims"));

            builder.Entity<IdentityUserLogin<string>>(
                entity => entity.ToTable("UserLogins"));

            builder.Entity<IdentityUserToken<string>>(
                entity => entity.ToTable("UserToken"));

            builder.Entity<IdentityRoleClaim<string>>(
                entity => entity.ToTable("RoleClaim"));

            builder.Entity<PermissionEf>(
                entity => entity.ToTable("Permissions"));

            builder.Entity<RolePermission>(
                entity => entity.ToTable("RolePermissions"));

            builder.Entity<SongEf>(
                s => s.Property(p => p.PublishedOn)
                        .HasColumnType("date"));

            builder.Entity<SongEf>()
                .HasOne(s => s.Uploader)
                .WithMany(u => u.Songs)
                .HasForeignKey(s => s.UploaderId);

            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RoleEf>()
                .HasMany(r => r.Permissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<PermissionEf>()
                .HasMany(p => p.Roles)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<AuditEntryEf>()
                .HasOne(ae => ae.User)
                .WithMany()
                .HasForeignKey(ae => ae.UserId);
        }
    }
}
