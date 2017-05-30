using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ReportExtension
    {
        public static void AddTests(this Report entry,
                                    IEnumerable<Test> testList)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Reports.Attach(entry);
                foreach (Test tst in testList)
                    entry.Tests.Add(tst);

                entities.SaveChanges();
            }
        }

        public static void Create(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Report tempEntry = new Report();
                entities.Reports.Add(tempEntry);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();

                entry.ID = tempEntry.ID;
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
