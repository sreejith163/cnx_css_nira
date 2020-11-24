using Css.Api.SetupMenu.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Css.Api.SetupMenu.Repository.DatabaseContext
{
    public partial class SetupMenuContext : DbContext
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupMenuContext"/> class.
        /// </summary>
        public SetupMenuContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupMenuContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configuration">The configuration.</param>
        public SetupMenuContext(
                    DbContextOptions<SetupMenuContext> options, IConfiguration configuration)
                    : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<AgentAdmin> AgentAdmin { get; set; }
        public virtual DbSet<AgentDetail> AgentDetail { get; set; }
        public virtual DbSet<AgentGroupDetail> AgentGroupDetail { get; set; }
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
            modelBuilder.Entity<AgentAdmin>(entity =>
            {
                entity.ToTable("agent_admin");

                entity.HasIndex(e => e.AgnetSchedulingGroupId)
                    .HasName("fk_agentadmin_agent_scheduling_group_agent_scheduling_group_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentSso)
                    .IsRequired()
                    .HasColumnName("agent_sso")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgnetSchedulingGroupId)
                    .HasColumnName("agnet_scheduling_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HireDate).HasColumnName("hire_date");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.HasOne(d => d.AgnetSchedulingGroup)
                    .WithMany(p => p.AgentAdmin)
                    .HasForeignKey(d => d.AgnetSchedulingGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_agentadmin_agent_scheduling_group_agent_scheduling_group_tid");
            });

            modelBuilder.Entity<AgentDetail>(entity =>
            {
                entity.ToTable("agent_detail");

                entity.HasIndex(e => e.SkillTagId)
                    .HasName("fk_agentdetail_skilltag_skilltagid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AcdId)
                    .HasColumnName("acd_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentCategoryId)
                    .HasColumnName("agent_category_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentLogon)
                    .HasColumnName("agent_logon")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MuValue)
                    .HasColumnName("mu_value")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenDateDay)
                    .HasColumnName("sen_date_day")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenDateMonth)
                    .HasColumnName("sen_date_month")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenDateYear)
                    .HasColumnName("sen_date_year")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SenExt)
                    .HasColumnName("sen_ext")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SkillTagId)
                    .HasColumnName("skill_tag_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ssn)
                    .HasColumnName("ssn")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartId)
                    .HasColumnName("start_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.SkillTag)
                    .WithMany(p => p.AgentDetail)
                    .HasForeignKey(d => d.SkillTagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_agentdetail_skilltag_skilltagid");
            });

            modelBuilder.Entity<AgentGroupDetail>(entity =>
            {
                entity.ToTable("agent_group_detail");

                entity.HasIndex(e => e.AgentId)
                    .HasName("fk_agentgroupdetail_agentdetail_agentid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentGroupDescription)
                    .HasColumnName("agent_group_description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgentGroupId)
                    .HasColumnName("agent_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentGroupValue)
                    .HasColumnName("agent_group_value")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgentId)
                    .HasColumnName("agent_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Agent)
                    .WithMany(p => p.AgentGroupDetail)
                    .HasForeignKey(d => d.AgentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_agentgroupdetail_agentdetail_agentid");
            });

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