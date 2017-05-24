using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ReportService
    {

        // TO BE MOVED IN SEPARATE CLASS
            
        public static List<Requirement> GenerateRequirementList(SpecificationVersion version)
        {
            if (version.IsMain)
                return new List<Requirement>(version.Requirements);

            else
            {
                List<Requirement> output = new List<Requirement>(
                    version.Specification.SpecificationVersions.First(sv => sv.IsMain).Requirements);

                foreach (Requirement requirement in version.Requirements)
                {
                    int ii = output.FindIndex(rr => rr.Method.ID == requirement.Method.ID);
                    output[ii] = requirement;
                }

                return output;
            }
        }

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

        public static void Create(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Reports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Reports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;

                entities.Reports.Attach(entry);

                Report tempEntry = entities.Reports
                                            .Include(rep => rep.Batch)
                                            .Include(rep => rep.ParentTask)
                                            .Include(rep => rep.SpecificationIssues)
                                            .Include(rep => rep.Tests)
                                            .Include(rep => rep.SpecificationVersion)
                                            .First(rep => rep.ID == entryID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);

            }
        }

        public static void Update(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                entities.Configuration.AutoDetectChangesEnabled = false;

                Report tempEntry = entities.Reports.First(rep => rep.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion

    }
}
