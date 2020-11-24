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

        public virtual DbSet<AgentAdmin> AgentAdmin { get; set; }
        public virtual DbSet<AgentCategory> AgentCategory { get; set; }
        public virtual DbSet<AgentCategoryDataType> AgentCategoryDataType { get; set; }
        public virtual DbSet<AgentDetail> AgentDetail { get; set; }
        public virtual DbSet<AgentGroupDetail> AgentGroupDetail { get; set; }
        public virtual DbSet<AgentSchedulingChart> AgentSchedulingChart { get; set; }
        public virtual DbSet<AgentSchedulingDetail> AgentSchedulingDetail { get; set; }
        public virtual DbSet<AgentSchedulingGroup> AgentSchedulingGroup { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientLobGroup> ClientLobGroup { get; set; }
        public virtual DbSet<CssLanguage> CssLanguage { get; set; }
        public virtual DbSet<CssMenu> CssMenu { get; set; }
        public virtual DbSet<CssVariable> CssVariable { get; set; }
        public virtual DbSet<LanguageTranslation> LanguageTranslation { get; set; }
        public virtual DbSet<OperationHour> OperationHour { get; set; }
        public virtual DbSet<OperationHourOpenType> OperationHourOpenType { get; set; }
        public virtual DbSet<SchedulingCode> SchedulingCode { get; set; }
        public virtual DbSet<SchedulingCodeIcon> SchedulingCodeIcon { get; set; }
        public virtual DbSet<SchedulingCodeType> SchedulingCodeType { get; set; }
        public virtual DbSet<SchedulingStatus> SchedulingStatus { get; set; }
        public virtual DbSet<SchedulingTypeCode> SchedulingTypeCode { get; set; }
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

            modelBuilder.Entity<AgentCategory>(entity =>
            {
                entity.ToTable("agent_category");

                entity.HasIndex(e => e.DataTypeId)
                    .HasName("FK_agent_category_agent_category_data_type_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DataTypeId)
                    .HasColumnName("data_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DataTypeMaxValue)
                    .IsRequired()
                    .HasColumnName("data_type_max_value")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DataTypeMinValue)
                    .IsRequired()
                    .HasColumnName("data_type_min_value")
                    .HasMaxLength(255)
                    .IsUnicode(false);

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

                entity.HasOne(d => d.DataType)
                    .WithMany(p => p.AgentCategory)
                    .HasForeignKey(d => d.DataTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_category_agent_category_data_type_id");
            });

            modelBuilder.Entity<AgentCategoryDataType>(entity =>
            {
                entity.ToTable("agent_category_data_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(255)
                    .IsUnicode(false);
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

            modelBuilder.Entity<AgentSchedulingChart>(entity =>
            {
                entity.ToTable("agent_scheduling_chart");

                entity.HasIndex(e => e.AgentSchedulingDetailId)
                    .HasName("FK_agent_scheduling_chart_agent_scheduling_detail_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AgentSchedulingDetailId)
                    .HasColumnName("agent_scheduling_detail_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Day)
                    .HasColumnName("day")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EndTime)
                    .IsRequired()
                    .HasColumnName("end_time")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IconId)
                    .HasColumnName("icon_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IconValue)
                    .IsRequired()
                    .HasColumnName("icon_value")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Meridian)
                    .IsRequired()
                    .HasColumnName("meridian")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.StartTime)
                    .IsRequired()
                    .HasColumnName("start_time")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.AgentSchedulingDetail)
                    .WithMany(p => p.AgentSchedulingChart)
                    .HasForeignKey(d => d.AgentSchedulingDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_chart_agent_scheduling_detail_id");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.AgentSchedulingChart)
                    .HasForeignKey<AgentSchedulingChart>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_chart_icon_id");
            });

            modelBuilder.Entity<AgentSchedulingDetail>(entity =>
            {
                entity.ToTable("agent_scheduling_detail");

                entity.HasIndex(e => e.SchedulingGroupId)
                    .HasName("FK_agent_scheduling_detail_scheduling_group_id_idx");

                entity.HasIndex(e => e.SchedulingStatusId)
                    .HasName("FK_agent_scheduling_detail_scheduling_status_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentId)
                    .HasColumnName("agent_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AgentName)
                    .IsRequired()
                    .HasColumnName("agent_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DateFrom).HasColumnName("date_from");

                entity.Property(e => e.DateTo).HasColumnName("date_to");

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
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SchedulingGroupId)
                    .HasColumnName("scheduling_group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SchedulingStatusId)
                    .HasColumnName("scheduling_status_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.SchedulingGroup)
                    .WithMany(p => p.AgentSchedulingDetail)
                    .HasForeignKey(d => d.SchedulingGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_detail_scheduling_group_id");

                entity.HasOne(d => d.SchedulingStatus)
                    .WithMany(p => p.AgentSchedulingDetail)
                    .HasForeignKey(d => d.SchedulingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_agent_scheduling_detail_scheduling_status_id");
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

            modelBuilder.Entity<CssLanguage>(entity =>
            {
                entity.ToTable("css_language");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CssMenu>(entity =>
            {
                entity.ToTable("css_menu");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CssVariable>(entity =>
            {
                entity.ToTable("css_variable");

                entity.HasIndex(e => e.MenuId)
                    .HasName("d_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasColumnName("menu_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.CssVariable)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_css_variable_css_menu");
            });

            modelBuilder.Entity<LanguageTranslation>(entity =>
            {
                entity.ToTable("language_translation");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("FK_language_translation_css_language_idx");

                entity.HasIndex(e => e.MenuId)
                    .HasName("FK_language_translation_css_menu_idx");

                entity.HasIndex(e => e.VariableId)
                    .HasName("FK_language_translation_css_variable_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LanguageName)
                    .HasColumnName("language_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MenuName)
                    .HasColumnName("menu_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Translation)
                    .IsRequired()
                    .HasColumnName("translation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VariableId)
                    .HasColumnName("variable_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.VariableName)
                    .HasColumnName("variable_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.LanguageTranslation)
                    .HasForeignKey(d => d.LanguageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_language_translation_css_language");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.LanguageTranslation)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_language_translation_css_menu");

                entity.HasOne(d => d.Variable)
                    .WithMany(p => p.LanguageTranslation)
                    .HasForeignKey(d => d.VariableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_language_translation_css_variable");
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

            modelBuilder.Entity<SchedulingCode>(entity =>
            {
                entity.ToTable("scheduling_code");

                entity.HasIndex(e => e.IconId)
                    .HasName("fk_schedulingcode_schedulingcodeicon_iconid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasColumnName("created_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IconId)
                    .HasColumnName("icon_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modified_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PriorityNumber)
                    .HasColumnName("priority_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RefId)
                    .HasColumnName("ref_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Icon)
                    .WithMany(p => p.SchedulingCode)
                    .HasForeignKey(d => d.IconId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_schedulingcode_schedulingcodeicon_iconid");
            });

            modelBuilder.Entity<SchedulingCodeIcon>(entity =>
            {
                entity.ToTable("scheduling_code_icon");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchedulingCodeType>(entity =>
            {
                entity.ToTable("scheduling_code_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchedulingStatus>(entity =>
            {
                entity.ToTable("scheduling_status");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SchedulingTypeCode>(entity =>
            {
                entity.ToTable("scheduling_type_code");

                entity.HasIndex(e => e.SchedulingCodeId)
                    .HasName("FK_scheduling_type_code_scheduling_code_id_idx");

                entity.HasIndex(e => e.SchedulingCodeTypeId)
                    .HasName("fk_schedulingcodeid_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SchedulingCodeId)
                    .HasColumnName("scheduling_code_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SchedulingCodeTypeId)
                    .HasColumnName("scheduling_code_type_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.SchedulingCode)
                    .WithMany(p => p.SchedulingTypeCode)
                    .HasForeignKey(d => d.SchedulingCodeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_scheduling_type_code_scheduling_code_id");

                entity.HasOne(d => d.SchedulingCodeType)
                    .WithMany(p => p.SchedulingTypeCode)
                    .HasForeignKey(d => d.SchedulingCodeTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_scheduling_type_code_type_code_id");
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