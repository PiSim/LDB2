using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ExternalReportExtension
    {
        public static void AddBatch(this ExternalReport entry,
                                    Batch batchEntity)
        {
            // Adds a Batch to an ExternalReport entry

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.First(ext => ext.ID == entry.ID)
                                        .Batches
                                        .Add(entities.Batches
                                        .First(btc => btc.ID == batchEntity.ID));

                entities.SaveChanges();
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
                
                entities.Entry(entities.ExternalReports
                        .First(ext => ext.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<Batch> GetBatches(this ExternalReport entry)
        {
            // Returns all Batch entities for an ExternalReport entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Where(btc => btc.ExternalReports
                                        .Any(ext => ext.ID == entry.ID))
                                        .ToList();
            }
        }

        public static IEnumerable<ExternalReportFile> GetExternalReportFiles(this ExternalReport entry)
        {
            // Returns all the files for an external report entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReportFiles.Where(extf => extf.ExternalReportID == entry.ID)
                                .ToList();
            }
        }

        public static void GetNumberAndCreate(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.Add(entry);
                entities.SaveChanges();
            }
        }


        public static void Load(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                ExternalReport tempEntry = entities.ExternalReports
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Aspect))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.Project))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.MaterialLine))
                                                    .Include(exrep => exrep.Batches
                                                    .Select(btc => btc.Material.MaterialType))
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
                                                    .First(rep => rep.ID == entry.ID);

                entry.ArrivalDate = tempEntry.ArrivalDate;
                entry.Batches = tempEntry.Batches;
                entry.Currency = tempEntry.Currency;
                entry.Description = tempEntry.Description;
                entry.ExternalLab = tempEntry.ExternalLab;
                entry.ExternalLabID = tempEntry.ExternalLabID;
                entry.ExternalNumber = tempEntry.ExternalNumber;
                entry.InternalNumber = tempEntry.InternalNumber;
                entry.MaterialSent = tempEntry.MaterialSent;
                entry.Methods = tempEntry.Methods;
                entry.PO = tempEntry.PO;
                entry.Price = tempEntry.Price;
                entry.Project = tempEntry.Project;
                entry.ProjectID = tempEntry.ProjectID;
                entry.PurchaseOrder = tempEntry.PurchaseOrder;
                entry.PurchaseOrderID = tempEntry.PurchaseOrderID;
                entry.ReportReceived = tempEntry.ReportReceived;
                entry.RequestDone = tempEntry.RequestDone;
                entry.Samples = tempEntry.Samples;
            }
        }

        public static void Update(this ExternalReport entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
