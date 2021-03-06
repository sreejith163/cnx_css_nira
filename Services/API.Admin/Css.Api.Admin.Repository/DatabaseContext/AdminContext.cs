﻿using Css.Api.Admin.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Css.Api.Admin.Repository.DatabaseContext
{
    public partial class AdminContext : DbContext
    {
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminContext"/> class.
        /// </summary>
        public AdminContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="configuration">The configuration.</param>
        public AdminContext(
                    DbContextOptions<AdminContext> options, IConfiguration configuration)
                    : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<AgentCategory> AgentCategory { get; set; }
        public virtual DbSet<AgentCategoryDataType> AgentCategoryDataType { get; set; }
        public virtual DbSet<CssLanguage> CssLanguage { get; set; }
        public virtual DbSet<CssMenu> CssMenu { get; set; }
        public virtual DbSet<CssVariable> CssVariable { get; set; }
        public virtual DbSet<LanguageTranslation> LanguageTranslation { get; set; }
        public virtual DbSet<SchedulingCode> SchedulingCode { get; set; }
        public virtual DbSet<SchedulingCodeIcon> SchedulingCodeIcon { get; set; }
        public virtual DbSet<SchedulingCodeType> SchedulingCodeType { get; set; }
        public virtual DbSet<SchedulingTypeCode> SchedulingTypeCode { get; set; }
        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<Log> Log { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL(_configuration.GetConnectionString("Database"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserLanguagePreference>(entity =>
            {
                entity.ToTable("user_language");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.LanguagePreference)
                    .HasColumnName("language_code")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("user_permissions");

                entity.HasIndex(e => e.UserRoleId)
                    .HasName("fk_role_id_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Sso)
                    .HasColumnName("sso")
                    .HasColumnType("varchar(50)");


                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Lastname)
                    .HasColumnName("lastname")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.HasOne(d => d.Role)
                      .WithMany()
                      .HasForeignKey(d => d.UserRoleId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("fk_role_id");

            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

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
                    .HasColumnName("data_type_max_value")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.DataTypeMinValue)
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
                    .HasName("FK_css_variable_css_menu_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("int(11)");

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

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MenuId)
                    .HasColumnName("menu_id")
                    .HasColumnType("int(11)");

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

                entity.Property(e => e.TimeOffCode)
                    .HasColumnName("time_off_code")
                    .HasColumnType("tinyint(4)");

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

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("user_logger");
                
                entity.Property(e => e.Id)
                 .HasColumnName("id")
                 .HasColumnType("int(11)");

                entity.Property(e => e.SSO)
                    .HasColumnName("sso")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.TimeStamp)
                .HasColumnName("time_stamp");

                entity.Property(e => e.UserAgent)
               .HasColumnName("user_agent");


            });

            modelBuilder.Entity<NonSsoModel>(entity =>
            {
                entity.ToTable("non_sso_auth");

                entity.Property(e => e.Id)
                 .HasColumnName("id")
                 .HasColumnType("int(11)");

                entity.Property(e => e.EmployeeId)
                 .HasColumnName("employee_id")
                 .HasColumnType("varchar(45)");

                entity.Property(e => e.RoleId)
                .HasColumnName("role_id")
                .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .HasColumnName("user_name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Sso)
                  .HasColumnName("sso")
                  .HasColumnType("varchar(45)");

                entity.Property(e => e.Firstname)
                    .HasColumnName("firstname")
                    .HasColumnType("varchar(45)");
                entity.Property(e => e.Lastname)
                 .HasColumnName("lastname")
                 .HasColumnType("varchar(45)");
                entity.Property(e => e.Password)
                 .HasColumnName("password")
                 .HasColumnType("varchar(45)");
                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted")
                    .HasColumnType("tinyint(4)");


            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}