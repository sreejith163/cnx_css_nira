using Css.Api.Scheduling.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Css.Api.Scheduling.Repository.DatabaseContext
{
    public partial class SchedulingContext : DbContext
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingContext"/> class.
        /// </summary>
        public SchedulingContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configuration">The configuration.</param>
        public SchedulingContext(
                    DbContextOptions<SchedulingContext> options, IConfiguration configuration)
                    : base(options)
        {
            _configuration = configuration;
        }
        public virtual DbSet<Timezone> Timezone { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_configuration.GetConnectionString("Database"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Timezone>(entity =>
            {
                entity.ToTable("timezone");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abbreviation)
                    .HasColumnName("abbreviation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasColumnName("display_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Offset)
                    .HasColumnName("offset")
                    .HasColumnType("int(11)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}