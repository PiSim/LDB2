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

        /// <summary>
        /// Calculates the total Duration of the tests associated with this report and stores the result
        /// in the corresponding field
        /// </summary>
        public void GetDuration()
        {
            using (DBEntities entities = new DBEntities())
            {
                IQueryable<Test> testList = entities.Tests
                                                    .Where(tst => tst.ReportID == ID);

                TotalDuration = (testList.Count() == 0) ? 0 : testList.Sum(tst => tst.Duration);
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
                                            .Include(rep => rep.Batch.Material.Project.Oem)
                                            .Include(rep => rep.Batch.Material.Project.Leader)
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
        /// Checks if the corresponding batch references a basic report
        /// If not, sets reference to this instance
        /// </summary>
        public void SetAsBasicIfNoReport()
        {
            using (DBEntities entities = new DBEntities())
            {
                Batch batchInstance = entities.Batches.First(btc => btc.ID == BatchID);
                if (batchInstance.BasicReportID == null)
                {
                    batchInstance.BasicReportID = ID;
                    entities.SaveChanges();
                }
            }
        }
    }

    public static class ReportExtension
    {
        public static bool AreTestsComplete(this Report entry)
        {
            // Returns true if all the tests are completed

            using (DBEntities entities = new DBEntities())
            {
                return entities.Tests.Where(tst => tst.ReportID == entry.ID)
                                    .All(tst => tst.IsComplete == true);
            }
        }

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
                                    .Include(tst => tst.Person)
                                    .Include(tst => tst.SubTests)
                                    .Where(tst => tst.ReportID == entry.ID)
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
                                .Where(tst => tst.ReportID == entry.ID)
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

        public static void UpdateTests(this Report entry)
        {
            // Updates all related Test and Subtest instances in a report

            using (DBEntities entities = new DBEntities())
            {
                foreach (Test tst in entry.Tests)
                {
                    entities.Tests.AddOrUpdate(tst);
                    foreach (SubTest sts in tst.SubTests)
                        entities.SubTests.AddOrUpdate(sts);
                }
                entities.SaveChanges();
            }
        }
    }
}
