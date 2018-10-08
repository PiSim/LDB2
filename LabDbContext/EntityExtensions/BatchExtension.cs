using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class BatchExtension
    {
        #region Methods

        public static void Create(this Batch entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Batches.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Batch entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                Batch tempEntry = entities.Batches.First(btc => btc.ID == entry.ID);

                entities.Entry(tempEntry).State = EntityState.Deleted;
                entry.ID = 0;

                entities.SaveChanges();
            }
        }

        public static void Update(this Batch entry)
        {
            // Updates the DB values of a Batch entity

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Batches.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class Batch
    {
        #region Properties

        public string AspectCode => Material?.Aspect?.Code;

        public bool HasTests
        {
            get
            {
                using (LabDbEntities entities = new LabDbEntities())
                {
                    return entities.TestRecords.Any(tstr => tstr.BatchID == ID);
                }
            }
        }

        public string MaterialFullCode => Material?.MaterialType?.Code
                                        + Material?.MaterialLine?.Code
                                        + Material?.Aspect?.Code
                                        + Material?.Recipe?.Code;

        public string MaterialLineCode => Material?.MaterialLine?.Code;
        public string MaterialTypeCode => Material?.MaterialType?.Code;
        public string RecipeCode => Material?.Recipe?.Code;

        #endregion Properties

    }
}