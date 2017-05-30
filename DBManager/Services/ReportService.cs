using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ReportService
    {
        #region Operations for ExternalReport entities

        public static void AddFile(this ExternalReport entry,
                                    ExternalReportFile fileEntry)
        {
            // Adds an ExternalReportFile entity to an ExternalReport

            if (entry == null || fileEntry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.Attach(entry);
                entry.ExternalReportFiles.Add(fileEntry);
                entities.SaveChanges();
            }
        }

        public static IEnumerable<ExternalReport> GetExternalReports()
        {
            // Returns all external report instances

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReports.Include(exrep => exrep.ExternalLab)
                                                .OrderBy(exrep => exrep.InternalNumber)
                                                .ToList();
            }
        }

        public static void Create(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.ExternalReports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.ExternalReports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;

                entities.ExternalReports.Attach(entry);

                ExternalReport tempEntry = entities.ExternalReports
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Construction.Aspect))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Construction.Project))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Construction.Type))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Recipe.Colour))
                                                    .Include(exrep => exrep.ExternalReportFiles)
                                                    .Include(exrep => exrep.ExternalLab)
                                                    .Include(exrep => exrep.Methods
                                                    .Select(mtd => mtd.Property))
                                                    .Include(exrep => exrep.Methods
                                                    .Select(mtd => mtd.Standard))
                                                    .Include(exrep => exrep.PO.Currency)
                                                    .Include(exrep => exrep.PO.Organization)
                                                    .Include(exrep => exrep.PO.PoFile)
                                                    .Include(exrep => exrep.Project.Leader)
                                                    .Include(exrep => exrep.Project.Oem)
                                                    .First(rep => rep.ID == entryID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                entities.Configuration.AutoDetectChangesEnabled = false;

                ExternalReport tempEntry = entities.ExternalReports.First(exrep => exrep.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for ExternalReportFile entities 

        public static void Delete(this ExternalReportFile entry)
        {
            // Deletes a given ExternalReportFile entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReportFiles.Attach(entry);
                entities.Entry(entry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Report entities

        public static Report GetReport(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<Report> GetReports()
        {
            // Returns all Report entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.AsNoTracking()
                                        .Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Construction.Aspect)
                                        .Include(rep => rep.Batch.Material.Construction.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Construction.Type)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.CurrentIssue)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.Organization)
                                        .Where(rep => true)
                                        .OrderByDescending(rep => rep.Number)
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
                                            .Include(rep => rep.ReportFiles)
                                            .Include(rep => rep.SpecificationIssues)
                                            .Include(rep => rep.Tests
                                            .Select(tst => tst.instrument.InstrumentType))
                                            .Include(rep => rep.Tests
                                            .Select(tst => tst.Method.Property))
                                            .Include(rep => rep.Tests
                                            .Select(tst => tst.Method.Standard.Organization))
                                            .Include(rep => rep.Tests
                                            .Select(tst => tst.SubTests))
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
                entry.ReportFiles = tempEntry.ReportFiles;
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
        #endregion

    }
}
