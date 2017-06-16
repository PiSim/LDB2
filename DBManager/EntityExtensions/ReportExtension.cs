using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
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
                                    .Include(tst => tst.MethodIssue)
                                    .Include(tst => tst.Person)
                                    .Include(tst => tst.SubTests)
                                    .Where(tst => tst.ReportID == entry.ID)
                                    .ToList();
            }
        }

        public static void Load(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Report tempEntry = entities.Reports
                                            .AsNoTracking()
                                            .Include(rep => rep.Author)
                                            .Include(rep => rep.Batch.Material.Construction.Aspect)
                                            .Include(rep => rep.Batch.Material.Construction.ExternalConstruction)
                                            .Include(rep => rep.Batch.Material.Construction.Project.Oem)
                                            .Include(rep => rep.Batch.Material.Construction.Project.Leader)
                                            .Include(rep => rep.Batch.Material.Construction.Type)
                                            .Include(rep => rep.Batch.Material.Recipe.Colour)
                                            .Include(rep => rep.ParentTask)
                                            .Include(rep => rep.SpecificationIssues)
                                            .Include(rep => rep.Tests)
                                            .Include(rep => rep.SpecificationVersion.Specification.Standard.Organization)
                                            .First(rep => rep.ID == entry.ID);

                entry.Author = tempEntry.Author;
                entry.AuthorID = tempEntry.AuthorID;
                entry.Batch = tempEntry.Batch;
                entry.BatchID = tempEntry.BatchID;
                entry.Category = tempEntry.Category;
                entry.Description = tempEntry.Description;
                entry.EndDate = tempEntry.EndDate;
                entry.IsComplete = tempEntry.IsComplete;
                entry.Number = tempEntry.Number;
                entry.ParentTask = tempEntry.ParentTask;
                entry.ParentTaskID = tempEntry.ParentTaskID;
                entry.SpecificationIssueID = tempEntry.SpecificationIssueID;
                entry.SpecificationIssues = tempEntry.SpecificationIssues;
                entry.SpecificationVersion = tempEntry.SpecificationVersion;
                entry.SpecificationVersionID = tempEntry.SpecificationVersionID;
                entry.StartDate = tempEntry.StartDate;
                entry.Tests = tempEntry.Tests;
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
