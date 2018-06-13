using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Method
    {

        /// <summary>
        /// Generates a new Test entry from the Method.
        /// The Method must be loaded.
        /// </summary>
        /// <returns>A newly generated, unattached Test entry</returns>
        public Test GenerateTest()
        {
            Test tempTest = new Test();
            tempTest.Duration = Duration;
            tempTest.MethodID = ID;
            tempTest.Notes = "";

            foreach (SubMethod subMtd in SubMethods)
            {
                SubTest tempSubTest = new SubTest()
                {
                    SubMethodID = subMtd.ID,
                    Name = subMtd.Name,
                    Position = subMtd.Position,
                    RequiredValue = "",
                    UM = subMtd.UM
                };
                tempTest.SubTests.Add(tempSubTest);
            }

            return tempTest;
        }

        /// <summary>
        /// Returns all the submethods for this entry, ordered by position
        /// </summary>
        /// <returns>an IList of Submethods</returns>
        public IList<SubMethod> GetSubMethods()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SubMethods.Where(smtd => smtd.MethodID == ID)
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
                                                    .Include(mtd => mtd.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(mtd => mtd.Property)
                                                    .Include(mtd => mtd.Standard.Organization);

                if (includeSubTests)
                    query = query.Include(mtd => mtd.SubMethods);

                Method tempEntry = query.First(spec => spec.ID == ID);

                AssociatedInstruments = tempEntry.AssociatedInstruments;
                Duration = tempEntry.Duration;
                Description = tempEntry.Description;
                ExternalReports = tempEntry.ExternalReports;
                Property = tempEntry.Property;
                PropertyID = tempEntry.PropertyID;
                Requirements = tempEntry.Requirements;
                Standard = tempEntry.Standard;
                StandardID = tempEntry.StandardID;

                if (includeSubTests)
                    SubMethods = tempEntry.SubMethods;

                TBD = "";
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

        public static IEnumerable<Test> GetResults(this Method entry)
        {
            // Returns all Tests for a given Method entry

            if (entry == null)
                return new List<Test>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests.Include(tst => tst.SubTests)
                                    .Where(tst => tst.MethodID == entry.ID)
                                    .ToList();
            }
        }

        public static IEnumerable<Specification> GetSpecifications(this Method entry)
        {
            // Returns all the specifications referring to the given method

            if (entry == null)
                return new List<Specification>();

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
                                .MethodID == entry.ID)))
                                .ToList();
            }
        }

        public static IEnumerable<Test> GetTests(this Method entry)
        {
            // Returns all Test entities for a Method

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests.Include(tst => tst.Method.Property)
                                    .Include(tst => tst.Method.Standard)
                                    .Include(tst => tst.TestRecord.Batch)
                                    .Include(tst => tst.SubTests)
                                    .Where(tst => tst.MethodID == entry.ID)
                                    .ToList();
            }
        }

        

        public static void LoadSubMethods(this Method entry)
        {
            // Loads related sub methods in a given method entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Method tempEntry = entities.Methods.Include(mtd => mtd.SubMethods)
                                                    .First(mtd => mtd.ID == entry.ID);

                entry.SubMethods = tempEntry.SubMethods;
            }
        }
    }
}
