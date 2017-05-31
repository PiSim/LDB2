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

        public static void SetAuthor(this Report entry,
                                    Person personEntity)
        {
            entry.Author = personEntity;
            entry.AuthorID = (personEntity == null) ? 0 : personEntity.ID;
        }

        public static void SetBatch(this Report entry,
                                    Batch batchEntity)
        {
            entry.Batch = batchEntity;
            entry.BatchID = (batchEntity == null) ? 0 : batchEntity.ID;
        }

        public static void SetSpecificationVersion(this Report entry,
                                                    SpecificationVersion specificationVersionEntity)
        {
            entry.SpecificationVersion = specificationVersionEntity;
            entry.SpecificationVersionID = (specificationVersionEntity == null) ? 0 : specificationVersionEntity.ID;
        }
    }
}
