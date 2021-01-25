using Css.Api.Setup.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Css.Api.Setup.Repository.DatabaseContext
{
    public partial class SetupContext : DbContext
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupContext"/> class.
        /// </summary>
        public SetupContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configuration">The configuration.</param>
        public SetupContext(
                    DbContextOptions<SetupContext> options, IConfiguration configuration)
                    : base(options)
        {
            _configuration = configuration;
        }
        public virtual DbSet<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientLobGroup> ClientLobGroup { get; set; }
        public virtual DbSet<OperationHour> OperationHour { get; set; }
        public virtual DbSet<OperationHourOpenType> OperationHourOpenType { get; set; }
        public virtual DbSet<SkillGroup> SkillGroup { get; set; }
        public virtual DbSet<SkillTag> SkillTag { get; set; }
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

            modelBuilder.Entity<AgentSchedulingGroup>(entity =>
            {
                entity.ToTable("agent_scheduling_group");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_agent_scheduling_group_client_id_idx");

                entity.HasIndex(e => e.ClientLobGroupId)
                    .HasName("FK_agent_scheduling_group_client_lob_group_id_idx");

                entity.HasIndex(e => e.SkillGroupId)
                    .HasName("FK_agent_scheduling_skill_group_idd_idx");

                entity.HasIndex(e => e.SkillTagId)
                    .HasName("FK_agent_scheduling_skill_tag_id_idx");

                entity.HasIndex(e => e.TimezoneId)
                    .HasName("FK_agent_scheduling_timezone_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientLobGroupId)
                    .HasColumnName("client_lob_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FirstDayOfWeek)
                    .HasColumnName("first_day_of_week")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SkillGroupId)
                    .HasColumnName("skill_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SkillTagId)
                    .HasColumnName("skill_tag_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimezoneId)
                    .HasColumnName("timezone_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.AgentSchedulingGroup)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_group_client_id");

                entity.HasOne(d => d.ClientLobGroup)
                    .WithMany(p => p.AgentSchedulingGroup)
                    .HasForeignKey(d => d.ClientLobGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_group_client_lob_group_id");

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.AgentSchedulingGroup)
                    .HasForeignKey(d => d.SkillGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_skill_group_idd");

                entity.HasOne(d => d.SkillTag)
                    .WithMany(p => p.AgentSchedulingGroup)
                    .HasForeignKey(d => d.SkillTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_skill_tag_id");

                entity.HasOne(d => d.Timezone)
                    .WithMany(p => p.AgentSchedulingGroup)
                    .HasForeignKey(d => d.TimezoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_timezone_id");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<ClientLobGroup>(entity =>
            {
                entity.ToTable("client_lob_group");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_client_lob_group_client_id_idx");

                entity.HasIndex(e => e.TimezoneId)
                    .HasName("FK_client_lob_group_timezone_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FirstDayOfWeek)
                    .HasColumnName("first_day_of_week")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimezoneId)
                    .HasColumnName("timezone_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientLobGroup)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_client_lob_group_client_id");

                entity.HasOne(d => d.Timezone)
                    .WithMany(p => p.ClientLobGroup)
                    .HasForeignKey(d => d.TimezoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_client_lob_group_timezone_id");
            });

            modelBuilder.Entity<OperationHour>(entity =>
            {
                entity.ToTable("operation_hour");

                entity.HasIndex(e => e.OperationHourOpenTypeId)
                    .HasName("FK_operation_hour_operation_hour_type_id_idx");

                entity.HasIndex(e => e.SchedulingGroupId)
                    .HasName("FK_operation_hour_agent_scheduling_group_id_idx");

                entity.HasIndex(e => e.SkillGroupId)
                    .HasName("FK_operation_hour_skill_group_id_idx");

                entity.HasIndex(e => e.SkillTagId)
                    .HasName("FK_operation_hour_skill_tag_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasColumnType("int(11)");

                entity.Property(e => e.From)
                    .HasColumnName("from")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.OperationHourOpenTypeId)
                    .HasColumnName("operation_hour_open_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SchedulingGroupId)
                    .HasColumnName("scheduling_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SkillGroupId)
                    .HasColumnName("skill_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SkillTagId)
                    .HasColumnName("skill_tag_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.To)
                    .HasColumnName("to")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.OperationHourOpenType)
                    .WithMany(p => p.OperationHour)
                    .HasForeignKey(d => d.OperationHourOpenTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_operation_hour_operation_hour_type_id");

                entity.HasOne(d => d.SchedulingGroup)
                    .WithMany(p => p.OperationHour)
                    .HasForeignKey(d => d.SchedulingGroupId)
                    .HasConstraintName("FK_operation_hour_agent_scheduling_group_id");

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.OperationHour)
                    .HasForeignKey(d => d.SkillGroupId)
                    .HasConstraintName("FK_operation_hour_skill_group_id");

                entity.HasOne(d => d.SkillTag)
                    .WithMany(p => p.OperationHour)
                    .HasForeignKey(d => d.SkillTagId)
                    .HasConstraintName("FK_operation_hour_skill_tag_id");

            });

            modelBuilder.Entity<OperationHourOpenType>(entity =>
            {
                entity.ToTable("operation_hour_open_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(2555)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SkillGroup>(entity =>
            {
                entity.ToTable("skill_group");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_skill_group_client_id_idx");

                entity.HasIndex(e => e.ClientLobGroupId)
                    .HasName("FK_skill_group_client_lob_group_id_idx");

                entity.HasIndex(e => e.TimezoneId)
                    .HasName("FK_skill_group_timezone_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientLobGroupId)
                    .HasColumnName("client_lob_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FirstDayOfWeek)
                    .HasColumnName("first_day_of_week")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TimezoneId)
                    .HasColumnName("timezone_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SkillGroup)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_group_client_id");

                entity.HasOne(d => d.ClientLobGroup)
                    .WithMany(p => p.SkillGroup)
                    .HasForeignKey(d => d.ClientLobGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_group_client_lob_group_id");

                entity.HasOne(d => d.Timezone)
                    .WithMany(p => p.SkillGroup)
                    .HasForeignKey(d => d.TimezoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_group_timezone_id");
            });

            modelBuilder.Entity<SkillTag>(entity =>
            {
                entity.ToTable("skill_tag");

                entity.HasIndex(e => e.ClientId)
                    .HasName("FK_skill_tag_client_id_idx");

                entity.HasIndex(e => e.ClientLobGroupId)
                    .HasName("FK_skill_tag_client_lob_group_id_idx");

                entity.HasIndex(e => e.SkillGroupId)
                    .HasName("FK_skill_tag_skill_group_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientId)
                    .HasColumnName("client_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClientLobGroupId)
                    .HasColumnName("client_lob_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SkillGroupId)
                    .HasColumnName("skill_group_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.SkillTag)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_tag_client_id");

                entity.HasOne(d => d.ClientLobGroup)
                    .WithMany(p => p.SkillTag)
                    .HasForeignKey(d => d.ClientLobGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_tag_client_lob_group_id");

                entity.HasOne(d => d.SkillGroup)
                    .WithMany(p => p.SkillTag)
                    .HasForeignKey(d => d.SkillGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skill_tag_skill_group_id");
            });

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