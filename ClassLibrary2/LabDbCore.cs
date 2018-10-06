using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabDbContextCore
{
    public partial class LabDbCore : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("metadata=res://*/LabDB.csdl|res://*/LabDB.ssdl|res://*/LabDB.msl;provider=MySql.Data.MySqlClient;provider connection string=&quot;server=192.168.1.22;user id=LabDBClient;Pwd=910938356;persistsecurityinfo=True;database=labdb;port=3306;SslMode=none&quot;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aspect>(entity =>
            {
                entity.ToTable("aspects");

                entity.HasKey(e => e.ID)
                    .HasName("ID");
                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .IsRequired()
                    .HasMaxLength(3);

                entity.HasMany(e => e.materials)
                        .WithOne(e => e.Aspect);
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("batches");

                entity.HasKey(e => e.ID)
                    .HasName("ID");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("number");

                entity.Property(e => e.MaterialID)
                    .IsRequired()
                    .HasColumnName("materialID");

                entity.Property(e => e.Notes)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("notes");

                entity.Property(e => e.TrialAreaID)
                    .HasColumnName("trial_areaID");

                entity.Property(e => e.FirstSampleArrived)
                    .IsRequired()
                    .HasColumnName("first_sample_arrived");

                entity.Property(e => e.FirstSampleID)
                    .HasColumnName("first_sampleID");

                entity.Property(e => e.BasicReportID)
                    .HasColumnName("basic_reportID");

                entity.Property(e => e.ArchiveStock)
                    .IsRequired()
                    .HasColumnName("archive_stock");

                entity.Property(e => e.DoNotTest)
                    .IsRequired()
                    .HasColumnName("do_not_test");
            });

            modelBuilder.Entity<BatchFile>(entity =>
            {
                entity.HasKey(e => e.ID)
                    .HasName("ID");

                entity.Property(e => e.BatchID)
                    .HasColumnName("batchID")
                    .IsRequired();

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .IsRequired();
            });
            modelBuilder.Entity<CalibrationFiles>(entity =>
            {
                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<CalibrationReport>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<CalibrationReportInstrumentPropertyMapping>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<CalibrationResult>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Colour>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ControlPlan>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ControlPlanItem>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ExternalConstruction>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ExternalReport>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ExternalReportFile>(entity =>
            {

                entity.HasKey(e => e.ID);
            });
            modelBuilder.Entity<Instrument>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<InstrumentFiles>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<InstrumentMaintenanceEvent>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<InstrumentMeasurableProperty>(entity =>
            {

                entity.HasKey(e => e.ID);
            });
            modelBuilder.Entity<InstrumentType>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<InstrumentUtilizationArea>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Master>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Material>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<MaterialLine>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<MaterialType>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<MeasurableQuantity>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<MeasurementUnit>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Method>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<MethodVariant>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Organization>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<OrganizationRole>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<OrganizationRoleMapping>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Person>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<PersonRole>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<PersonRoleMapping>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Project>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Property>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Recipe>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Report>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<ReportFile>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Requirement>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Sample>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Specification>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<SpecificationVersion>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<StandardFile>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Std>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<SubMethod>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<SubRequirement>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<SubTaskItem>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<SubTest>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Task>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<TaskItem>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<Test>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<TestRecord>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<TestRecordType>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<TrialArea>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<User>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {

                entity.HasKey(e => e.ID)
                    .HasName("ID");
            });
            modelBuilder.Entity<UserRoleMapping>(entity =>
            {

                entity.HasKey(e => e.ID);
            });
        }

        public virtual DbSet<Aspect> Aspects { get; set; }
        public virtual DbSet<BatchFile> BatchFiles { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Colour> Colours { get; set; }
        public virtual DbSet<ExternalConstruction> ExternalConstructions { get; set; }
        public virtual DbSet<ExternalReportFile> ExternalReportFiles { get; set; }
        public virtual DbSet<ExternalReport> ExternalReports { get; set; }
        public virtual DbSet<Master> Masters1 { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<ReportFile> ReportFiles { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Requirement> Requirements { get; set; }
        public virtual DbSet<Sample> Samples { get; set; }
        public virtual DbSet<SpecificationVersion> SpecificationVersions { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }
        public virtual DbSet<StandardFile> StandardFiles { get; set; }
        public virtual DbSet<Std> Stds { get; set; }
        public virtual DbSet<SubRequirement> SubRequirements { get; set; }
        public virtual DbSet<SubTest> SubTests { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<ControlPlan> ControlPlans { get; set; }
        public virtual DbSet<CalibrationFiles> CalibrationFiles { get; set; }
        public virtual DbSet<CalibrationReport> CalibrationReports { get; set; }
        public virtual DbSet<InstrumentType> InstrumentTypes { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }
        public virtual DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }
        public virtual DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public virtual DbSet<InstrumentMaintenanceEvent> InstrumentMaintenanceEvents { get; set; }
        public virtual DbSet<SubMethod> SubMethods { get; set; }
        public virtual DbSet<PersonRoleMapping> PersonRoleMappings { get; set; }
        public virtual DbSet<PersonRole> PersonRoles { get; set; }
        public virtual DbSet<SubTaskItem> SubTaskItems { get; set; }
        public virtual DbSet<MaterialLine> MaterialLines { get; set; }
        public virtual DbSet<TrialArea> TrialAreas { get; set; }
        public virtual DbSet<InstrumentUtilizationArea> InstrumentUtilizationAreas { get; set; }
        public virtual DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public virtual DbSet<InstrumentMeasurableProperty> InstrumentMeasurableProperties { get; set; }
        public virtual DbSet<MeasurableQuantity> MeasurableQuantities { get; set; }
        public virtual DbSet<CalibrationReportInstrumentPropertyMapping> CalibrationReportInstrumentPropertyMappings { get; set; }
        public virtual DbSet<CalibrationResult> CalibrationResults { get; set; }
        public virtual DbSet<InstrumentFiles> InstrumentFiles { get; set; }
        public virtual DbSet<ControlPlanItem> ControlPlanItems { get; set; }
        public virtual DbSet<TestRecord> TestRecords { get; set; }
        public virtual DbSet<TestRecordType> TestRecordTypes { get; set; }
        public virtual DbSet<MethodVariant> MethodVariants { get; set; }
    }
}
