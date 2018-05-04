using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Batch
    {
        public string MaterialFullCode => Material?.MaterialType?.Code
                                        + Material?.MaterialLine?.Code
                                        + Material?.Aspect?.Code
                                        + Material?.Recipe?.Code;

        /// <summary>
        /// Returns all the ExternalReport instances that refer to this batch
        /// </summary>
        /// <returns>An IEnumerable of Extenalreport entities</returns>
        public IEnumerable<ExternalReport> GetExternalReports()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReports
                                .Include(exr => exr.ExternalLab)
                                .Where(exr => exr.Batches
                                .Any(btc => btc.ID == ID))
                                .ToList();                                
            }
        }
    }

    public static class BatchExtension
    {
        public static Material GetMaterial(this Batch entry)
        {
            // Returns loaded material for batch entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.ExternalConstruction)
                                        .Include(mat => mat.Recipe.Colour)
                                        .FirstOrDefault(mat => mat.ID == entry.MaterialID);
            }
        }



        public static void Create(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                Batch tempEntry = entities.Batches.First(btc => btc.ID == entry.ID);

                entities.Entry(tempEntry).State = EntityState.Deleted;
                entry.ID = 0;

                entities.SaveChanges();
            }
        }

        public static IEnumerable<Sample> GetSamples(this Batch entry)
        {
            // Returns all Samples for a Batch

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Samples.Include(smp => smp.LogAuthor)
                                        .Where(smp => smp.BatchID == entry.ID)
                                        .OrderBy(smp => smp.Date)
                                        .ToList();
            }
        }

        public static void Load(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Batch tempEntry = entities.Batches.Include(btc => btc.BatchFiles)
                                                    .Include(btc => btc.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(btc => btc.Masters)
                                                    .Include(btc => btc.Material.Aspect)
                                                    .Include(btc => btc.Material.MaterialType)
                                                    .Include(btc => btc.Material.MaterialLine)
                                                    .Include(btc => btc.Material.Project.Leader)
                                                    .Include(btc => btc.Material.Project.Oem)
                                                    .Include(btc => btc.Material.Recipe.Colour)
                                                    .Include(btc => btc.Reports
                                                    .Select(rep => rep.Author))
                                                    .Include(btc => btc.Reports
                                                    .Select(rep => rep.SpecificationVersion.Specification.Standard))
                                                    .Include(btc => btc.Tasks
                                                    .Select(tsk => tsk.SpecificationVersion.Specification.Standard))
                                                    .FirstOrDefault(btc => btc.ID == entry.ID);

                entry.BatchFiles = tempEntry.BatchFiles;
                entry.ExternalReports = tempEntry.ExternalReports;
                entry.Masters = tempEntry.Masters;
                entry.Material = tempEntry.Material;
                entry.MaterialID = tempEntry.MaterialID;
                entry.Notes = tempEntry.Notes;
                entry.Number = tempEntry.Number;
                entry.Reports = tempEntry.Reports;
                entry.Samples = tempEntry.Samples;
                entry.Tasks = tempEntry.Tasks;
            }
        }

        public static void Update(this Batch entry)
        {
            // Updates the DB values of a Batch entity

            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
