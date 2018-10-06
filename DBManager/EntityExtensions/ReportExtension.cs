using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class ReportExtension
    {
        #region Methods

        public static void Create(this Report entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Reports.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Report entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Reports.First(rep => rep.ID == entry.ID)).State = EntityState.Deleted;
                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<ReportFile> GetReportFiles(this Report entry)
        {
            // Returns all ReportFiles for a report Entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ReportFiles.Where(repf => repf.reportID == entry.ID)
                                            .ToList();
            }
        }

        public static void Update(this Report entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Reports.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void UpdateDuration(this Report entry)
        {
            // Updates the value of the field "TotalDuration" with the sum of the report's tests individual durations

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Reports
                        .First(rep => rep.ID == entry.ID)
                        .TotalDuration = entities.Tests
                                                    .Where(tst => tst.TestRecordID == entry.TestRecordID)
                                                    .Sum(tst => tst.Duration);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class Report
    {
        #region Methods

        public Project GetProject()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.Include(rpt => rpt.Batch.Material.Project)
                                        .FirstOrDefault(rpt => rpt.ID == ID)?
                                        .Batch?
                                        .Material?
                                        .Project;
            }
        }

        public void Load()
        {
            // Retrieves Header information for the Report

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Report tempEntry = entities.Reports
                                            .AsNoTracking()
                                            .Include(rep => rep.Author)
                                            .Include(rep => rep.Batch.Material.Aspect)
                                            .Include(rep => rep.Batch.Material.ExternalConstruction)
                                            .Include(rep => rep.Batch.Material.MaterialLine)
                                            .Include(rep => rep.Batch.Material.MaterialType)
                                            .Include(rep => rep.Batch.Material.MaterialType)
                                            .Include(rep => rep.Batch.Material.Recipe.Colour)
                                            .Include(rep => rep.ParentTasks)
                                            .Include(rep => rep.SpecificationVersion.Specification.Standard.Organization)
                                            .First(rep => rep.ID == ID);

                Author = tempEntry.Author;
                AuthorID = tempEntry.AuthorID;
                Batch = tempEntry.Batch;
                BatchID = tempEntry.BatchID;
                Category = tempEntry.Category;
                Description = tempEntry.Description;
                EndDate = tempEntry.EndDate;
                IsComplete = tempEntry.IsComplete;
                Number = tempEntry.Number;
                ParentTasks = tempEntry.ParentTasks;
                SpecificationVersion = tempEntry.SpecificationVersion;
                SpecificationVersionID = tempEntry.SpecificationVersionID;
                StartDate = tempEntry.StartDate;
            }
        }

        /// <summary>
        /// Sets the project for the material of this report to the one specified
        /// as argument
        /// </summary>
        /// <param name="projectInstance">The project instance that will be set</param>
        public void SetProject(Project projectInstance)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                Material target = entities.Reports
                                            .FirstOrDefault(rpt => rpt.ID == ID)?
                                            .Batch
                                            .Material;

                target.ProjectID = projectInstance.ID;

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}