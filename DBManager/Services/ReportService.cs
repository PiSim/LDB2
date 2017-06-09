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
                entities.ExternalReports.Add(entry);
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

        #endregion
    }
}
