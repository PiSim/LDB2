using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class StandardExtension
    {
        #region Methods

        public static void Create(this Std entry)
        {
            // Inserts a new Std entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Stds.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this Std entry)
        {
            // Updates a STD entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Stds
                        .AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class Std
    {
        #region Methods

        /// <summary>
        /// Deletes the entry from the DB and sets the ID of the local instance to 0
        /// </summary>
        public void Delete()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                Std tempEntry = entities.Stds.First(std => std.ID == ID);

                entities.Entry(tempEntry).State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                ID = 0;
            }
        }

        /// <summary>
        /// Returns all the StandardFiles related to this standard
        /// </summary>
        /// <returns>An IEnumerable of StandardFiles entities</returns>
        public IEnumerable<StandardFile> GetFiles()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stf => stf.StandardID == ID)
                                .ToList();
            }
        }

        /// <summary>
        /// Returns all the Method entities related to this standard
        /// </summary>
        /// <returns>An IEnumerable of Method entities</returns>
        public IEnumerable<Method> GetMethods()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods
                                .Where(mtd => mtd.StandardID == ID)
                                .ToList();
            }
        }

        /// <summary>
        /// Returns all the Specifications related to this standard
        /// </summary>
        /// <returns>An IEnumerable of Specification entities</returns>
        public IEnumerable<Specification> GetSpecifications()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Specifications
                                .Where(spc => spc.StandardID == ID)
                                .ToList();
            }
        }

        #endregion Methods
    }
}