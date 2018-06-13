using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Report
    {
        public Project GetProject()
        {
            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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
            using (DBEntities entities = new DBEntities())
            {
                Material target = entities.Reports
                                            .FirstOrDefault(rpt => rpt.ID == ID)?
                                            .Batch
                                            .Material;
                
                target.ProjectID = projectInstance.ID;

                entities.SaveChanges();
            }
        }

    }

    public static class ReportExtension
    {
        public static void Create(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Reports.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ReportFiles.Where(repf => repf.reportID == entry.ID)
                                            .ToList();
            }
        }

        public static IEnumerable<Test> GetTests(this Report entry)
        {
            // Returns all tests for a Report Entry

            if (entry == null)
                return new List<Test>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests.Include(tst => tst.Instrument.InstrumentType)
                                    .Include(tst => tst.Method.Property)
                                    .Include(tst => tst.Method.Standard.Organization)
                                    .Include(tst => tst.SubTests)
                                    .Where(tst => tst.TestRecordID == entry.TestRecordID)
                                    .ToList();
            }
        }

        public static double GetTotalDuration(this Report entry)
        {
            //Returns the sum of the duration of all the tests in the report

            if (entry == null)
                return 0;

            using (DBEntities entities = new DBEntities())
            {
                return entities.Tests
                                .Where(tst => tst.TestRecordID == entry.TestRecordID)
                                .Sum(tst => tst.Method.Duration);
            }
        }

        public static void Update(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Reports.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void UpdateDuration(this Report entry)
        {
            // Updates the value of the field "TotalDuration" with the sum of the report's tests individual durations

            using (DBEntities entities = new DBEntities())
            {
                entities.Reports
                        .First(rep => rep.ID == entry.ID)
                        .TotalDuration = entities.Tests
                                                    .Where(tst => tst.TestRecordID == entry.TestRecordID)
                                                    .Sum(tst => tst.Duration);

                entities.SaveChanges();
            }
        }
    }
}
