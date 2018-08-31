using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace DBManager
{
    public partial class Method
    {        
        /// <summary>
        /// Retrieves the Specifications Utilizing this Method
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Specification> GetSpecifications()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Specifications
                                .Include(spec => spec.Standard.Organization)
                                .Where(spec => spec
                                .SpecificationVersions
                                .Any(specv => specv
                                .Requirements
                                .Any(req => req
                                .MethodVariant.MethodID == ID)))
                                .ToList();
            }
        }


        /// <summary>
        /// Retrieves all Test Entities for a Method
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Test> GetTests()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests.Include(tst => tst.MethodVariant.Method.Property)
                                    .Include(tst => tst.MethodVariant.Method.Standard)
                                    .Include(tst => tst.TestRecord.Batch)
                                    .Include(tst => tst.SubTests)
                                    .Where(tst => tst.MethodVariantID == ID)
                                    .ToList();
            }
        }
        
        /// <summary>
        /// Queries the database for related MethodVariants and returns them as ICollection
        /// </summary>
        /// <param name="includeObsolete"> If true returns all entries, otherwise the ones with the flag IsOld are excluded </parameter>
        /// <returns>An ICollection of the MethodVariant entities for this method Entry</returns>
        public ICollection<MethodVariant> GetVariants(bool includeObsolete = false)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MethodVariants
                                .Where(mtdvar => mtdvar.MethodID == ID)
                                .ToList();
            }
        }

        public void Load(bool includeSubTests = false)
        {

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<Method> query = entities.Methods.Include(mtd => mtd.AssociatedInstruments
                                                    .Select(inst => inst.InstrumentType))
                                                    .Include(mtd => mtd.Property)
                                                    .Include(mtd => mtd.Standard.Organization);

                if (includeSubTests)
                    query = query.Include(mtd => mtd.SubMethods);

                Method tempEntry = query.First(spec => spec.ID == ID);

                AssociatedInstruments = tempEntry.AssociatedInstruments;
                Duration = tempEntry.Duration;
                Description = tempEntry.Description;
                Property = tempEntry.Property;
                PropertyID = tempEntry.PropertyID;
                Standard = tempEntry.Standard;
                StandardID = tempEntry.StandardID;
                SubMethods = tempEntry.SubMethods.ToList();
                TBD = "";
            }
        }

        public void LoadSubMethods()
        {
            // Loads related sub methods in a given method entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                SubMethods = entities.Methods.Include(mtd => mtd.SubMethods)
                                            .FirstOrDefault(mtd => mtd.ID == ID)?
                                            .SubMethods
                                            .ToList();
            }
        }

        public void Update()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.AddOrUpdate(this);

                foreach (SubMethod smtd in SubMethods)
                    entities.SubMethods.AddOrUpdate(smtd);

                entities.SaveChanges();
            }
        }
    }

    public static class MethodExtension
    {
        public static void Create(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static IEnumerable<StandardFile> GetFiles(this Method entry)
        {
            // Returns all standard Files for a method standard

            if (entry == null)
                return new List<StandardFile>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stdf => stdf.StandardID == entry.StandardID)
                                .ToList();
            }
        }

        

    }
}
