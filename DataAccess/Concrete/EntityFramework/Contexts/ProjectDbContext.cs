using System;
using System.Reflection;
using Core.Entities.Concrete;
using DataAccess.Concrete.Configurations;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class ProjectDbContext : DbContext
    {
        /// <summary>
        /// Constructor for dependency injection with options.
        /// </summary>
        /// <param name="options"></param>
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        /// <summary>
        /// Default parameterless constructor.
        /// </summary>
        public ProjectDbContext()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MobileLogin> MobileLogins { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translate> Translates { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Progress> StudentProgress { get; set; }
        public DbSet<StudentSession> StudentSession { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }

        /// <summary>
        /// Configure entity mappings.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasDiscriminator<string>("Role")
                .HasValue<User>("Person")
                .HasValue<School>("School")
                .HasValue<Branch>("Branch")
                .HasValue<Student>("Student")
                .HasValue<Trainer>("Trainer");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Configure the database provider and connection string.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            if (!optionsBuilder.IsConfigured)
            {
                // Add your fallback connection string here if needed.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PineSport2;Username=emirsafayavuz;Password=12345")
                              .EnableSensitiveDataLogging();
            }
        }
    }
}