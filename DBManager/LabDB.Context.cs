﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
        public virtual DbSet<Construction> Constructions { get; set; }
        public virtual DbSet<ExternalConstruction> ExternalConstructions { get; set; }
        public virtual DbSet<ExternalReportFile> ExternalReportFiles { get; set; }
        public virtual DbSet<ExternalReport> ExternalReports { get; set; }
        public virtual DbSet<File> Files1 { get; set; }
        public virtual DbSet<Master> Masters1 { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<Oem> Oems1 { get; set; }
        public virtual DbSet<Organization> Organizations1 { get; set; }
        public virtual DbSet<Person> People1 { get; set; }
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
        public virtual DbSet<User> Users1 { get; set; }
    }
}
