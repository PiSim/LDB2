using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class MethodExtension
    {
        #region Methods

        public static IEnumerable<StandardFile> GetFiles(this Method entry)
        {
            // Returns all standard Files for a method standard

            if (entry == null)
                return new List<StandardFile>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stdf => stdf.StandardID == entry.StandardID)
                                .ToList();
            }
        }

        #endregion Methods
    }

    public partial class Method
    {
        #region Methods


        /// <summary>
        /// Queries the database for related MethodVariants and returns them as ICollection
        /// </summary>
        /// <param name="includeObsolete"> If true returns all entries, otherwise the ones with the flag IsOld are excluded </parameter>
        /// <returns>An ICollection of the MethodVariant entities for this method Entry</returns>
        public ICollection<MethodVariant> GetVariants(bool includeObsolete = false)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MethodVariants
                                .Where(mtdvar => mtdvar.MethodID == ID)
                                .ToList();
            }
        }

        public void LoadSubMethods()
        {
            // Loads related sub methods in a given method entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                SubMethods = entities.Methods.Include(mtd => mtd.SubMethods)
                                            .FirstOrDefault(mtd => mtd.ID == ID)?
                                            .SubMethods
                                            .ToList();
            }
        }
        
        #endregion Methods
    }
}