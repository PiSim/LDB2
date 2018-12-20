using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.Services
{
    public static class MaterialService
    {
        #region Operations for ExternalConstruction entities

        public static void AddMaterial(this ExternalConstruction entry,
                                            Material toBeAdded)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Materials.First(mat => mat.ID == toBeAdded.ID)
                                    .ExternalConstructionID = entry.ID;

                entities.SaveChanges();
            }
        }

        public static void Create(this ExternalConstruction entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ExternalConstructions.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this ExternalConstruction entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .ExternalConstructions
                        .First(exc => exc.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static void Update(this ExternalConstruction entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ExternalConstructions.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion Operations for ExternalConstruction entities
    }
}