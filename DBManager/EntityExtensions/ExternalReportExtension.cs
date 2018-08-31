using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class ExternalReport
    {
        #region Proprietà

        public string FormattedNumber
        {
            get { return Year.ToString() + Number.ToString("d3"); }
        }

        #endregion

        #region Metodi

        public void AddBatch(Batch batchEntity)
        {
            // Adds a Batch to an ExternalReport entry

            using (DBEntities entities = new DBEntities())
            {
                ExternalReport attachedEntry = entities.ExternalReports.First(ext => ext.ID == ID);

                TestRecord recordEntry = new TestRecord()
                {
                    BatchID = batchEntity.ID,
                    RecordTypeID = 2
                };

                foreach (MethodVariant mtdVar in attachedEntry.MethodVariants)
                    recordEntry.Tests.Add(mtdVar.GenerateTest());

                attachedEntry.TestRecords.Add(recordEntry);

                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a new method to an ExternalReport entry
        /// </summary>
        /// <param name="methodEntity">The MethodVariant to Add</param>
        public void AddTestMethod(MethodVariant methodVariant)
        {
            using (DBEntities entities = new DBEntities())
            {
                ExternalReport attachedExternalReport = entities.ExternalReports.First(ext => ext.ID == ID);
                MethodVariant attachedMethodVariant = entities.MethodVariants.First(mtd => mtd.ID == methodVariant.ID);
                
                attachedExternalReport.MethodVariants.Add(attachedMethodVariant);

                IEnumerable<TestRecord> recordList = attachedExternalReport.TestRecords.ToList();

                methodVariant.LoadMethod(true);

                foreach (TestRecord tstr in attachedExternalReport.TestRecords)
                    tstr.Tests.Add(methodVariant.GenerateTest());

                entities.SaveChanges();
            }
        }

        public void Create()
        {
            // Inserts the report in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.Add(this);
                entities.SaveChanges();
            }
        }
        
        /// <summary>
        /// Returns the method Variants associated with this External Report
        /// </summary>
        /// <returns>An ICollection containing the found MethodVariant entries</returns>
        public ICollection<MethodVariant> GetMethodVariants()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MethodVariants.Include(mtdvar => mtdvar.Method.Property)
                                            .Include(mtdvar => mtdvar.Method.Standard.Organization)
                                            .Include(mtdvar => mtdvar.Method.SubMethods)
                                            .Where(mtdvar => mtdvar.ExternalReports
                                            .Any(exr => exr.ID == ID))
                                            .ToList();
            }
        }

        /// <summary>
        /// Returns a list of test for each methodVariant associated with the Report 
        /// generated from the entity collections loaded in the instance
        /// </summary>
        /// <returns>An IEnumerable of Tuples where Value1 is a methodVAriant and 
        /// Value2 is an IEnumerable of tests</returns>
        public IEnumerable<Tuple<MethodVariant, IEnumerable<Test>>> GetResultCollection()
        {
            List<Tuple<MethodVariant, IEnumerable<Test>>> output = new List<Tuple<MethodVariant,IEnumerable<Test>>>();
            foreach (MethodVariant mtdvar in MethodVariants)
            {
                IEnumerable<Test> testList =  TestRecords.SelectMany(tstr => tstr.Tests)
                                                        .Where(tst => tst.MethodVariantID == mtdvar.ID);
                    
                output.Add(new Tuple<MethodVariant, IEnumerable<Test>>(mtdvar, testList));
            }

            return output;
        }
        
        /// <summary>
        /// Fetches the TestRecord entities for this ExternalReport
        /// </summary>
        /// <returns></returns>
        public ICollection<TestRecord> GetTestRecords(bool includeTests = false)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<TestRecord> query = entities.TestRecords.Include(tstr => tstr.Batch.Material.Aspect)
                                                                    .Include(tstr => tstr.Batch.Material.MaterialLine)
                                                                    .Include(tstr => tstr.Batch.Material.MaterialType)
                                                                    .Include(tstr => tstr.Batch.Material.Recipe.Colour);

                if (includeTests)
                    query = query.Include(tstr => tstr.Tests
                                                      .Select(tst => tst.SubTests));

                return query.Where(tstr => tstr.ExternalReports
                            .Any(extr => extr.ID == ID))
                            .ToList();
            }
        }

        /// <summary>
        /// Removes a given methodVariant from the method associations and from every test record
        /// in the report
        /// </summary>
        /// <param name="methodEntity">The methodVariant that will be removed</param>
        public void RemoveTestMethodVariant(MethodVariant methodVariantEntity)
        {
            using (DBEntities entities = new DBEntities())
            {
                ExternalReport attachedExternalReport = entities.ExternalReports.First(ext => ext.ID == ID);
                MethodVariant attachedMethodVariant = entities.MethodVariants.First(mtdvar => mtdvar.ID == methodVariantEntity.ID);

                attachedExternalReport.MethodVariants.Remove(attachedMethodVariant);

                IEnumerable<TestRecord> recordList = attachedExternalReport.TestRecords.ToList();

                IEnumerable<Test> testList = attachedExternalReport.TestRecords.SelectMany(tstr => tstr.Tests)
                                                                    .Where(tst => tst.MethodVariantID == methodVariantEntity.ID)
                                                                    .ToList();

                foreach (Test tst in testList)
                    entities.Entry(tst).State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Pushes the current Instance to the database updating all the values
        /// </summary>
        /// <param name="updateTests">If true all the related SubTest entities are updated too</param>
        public void Update(bool updateTests = false)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.AddOrUpdate(this);
                
                if (updateTests)
                    foreach (SubTest sts in TestRecords.SelectMany(tsr => tsr.Tests)
                                                       .SelectMany(tst => tst.SubTests)
                                                       .ToList())
                        entities.SubTests.AddOrUpdate(sts);
                
                entities.SaveChanges();
            }
        }

        #endregion
    }

    public static class ExternalReportExtension
    {

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
                                                    .Include(exrep => exrep.ExternalLab)
                                                    .Include(exrep => exrep.MethodVariants
                                                    .Select(mtdVar => mtdVar.Method.Property))
                                                    .Include(exrep => exrep.MethodVariants
                                                    .Select(mtdvar => mtdvar.Method.Standard))
                                                    .Include(exrep => exrep.Project.Leader)
                                                    .Include(exrep => exrep.Project.Oem)
                                                    .First(rep => rep.ID == entry.ID);

                entry.ArrivalDate = tempEntry.ArrivalDate;
                entry.Description = tempEntry.Description;
                entry.ExternalLab = tempEntry.ExternalLab;
                entry.ExternalLabID = tempEntry.ExternalLabID;
                entry.MaterialSent = tempEntry.MaterialSent;
                entry.MethodVariants = tempEntry.MethodVariants.ToList();
                entry.Project = tempEntry.Project;
                entry.ProjectID = tempEntry.ProjectID;
                entry.ReportReceived = tempEntry.ReportReceived;
                entry.RequestDone = tempEntry.RequestDone;
                entry.Samples = tempEntry.Samples;
            }
        }
               

    }
}
