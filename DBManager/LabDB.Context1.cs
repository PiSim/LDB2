﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LabDBEntities : DbContext
    {
        public LabDBEntities()
            : base("name=LabDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
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
        public virtual DbSet<StandardIssue> StandardIssues { get; set; }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<ControlPlanItem> ControlPlanItems1 { get; set; }
        public virtual DbSet<ControlPlan> ControlPlans { get; set; }
        public virtual DbSet<CalibrationFiles> CalibrationFiles { get; set; }
        public virtual DbSet<CalibrationReport> CalibrationReports { get; set; }
        public virtual DbSet<InstrumentType> InstrumentTypes { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }
        public virtual DbSet<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }
        public virtual DbSet<OrganizationRole> OrganizationRoles { get; set; }
        public virtual DbSet<PurchaseOrderFile> PurchaseOrderFiles { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<InstrumentMaintenanceEvent> InstrumentMaintenanceEvents { get; set; }
        public virtual DbSet<SubMethod> SubMethods { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
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
    }
}
