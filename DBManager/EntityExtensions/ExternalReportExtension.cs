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

                foreach (Method mtd in attachedEntry.Methods)
                    recordEntry.Tests.Add(mtd.GenerateTest());

                attachedEntry.TestRecords.Add(recordEntry);

                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a new method to an ExternalReport entry
        /// </summary>
        /// <param name="methodEntity">The Method to Add</param>
        public void AddTestMethod(Method methodEntity)
        {
            using (DBEntities entities = new DBEntities())
            {
                ExternalReport attachedExternalReport = entities.ExternalReports.First(ext => ext.ID == ID);
                Method attachedMethod = entities.Methods.First(mtd => mtd.ID == methodEntity.ID);
                
                attachedExternalReport.Methods.Add(attachedMethod);

                IEnumerable<TestRecord> recordList = attachedExternalReport.TestRecords.ToList();

                methodEntity.Load(true);

                foreach (TestRecord tstr in attachedExternalReport.TestRecords)
                    tstr.Tests.Add(methodEntity.GenerateTest());

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
        /// Returns the methods associated with this External Report
        /// </summary>
        /// <returns>An IEnumerable containing the found Method entries</returns>
        public IEnumerable<Method> GetMethods()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Property)
                                        .Include(mtd => mtd.Standard.Organization)
                                        .Include(mtd => mtd.SubMethods)
                                        .Where(mtd => mtd.ExternalReports
                                        .Any(exr => exr.ID == ID))
                                        .ToList();
            }
        }

        /// <summary>
        /// Returns a list of test for each method associated with the Report 
        /// generated from the entity collections loaded in the instance
        /// </summary>
        /// <returns>An IEnumerable of Tuples where Value1 is a method and 
        /// Value2 is an IEnumerable of tests</returns>
        public IEnumerable<Tuple<Method, IEnumerable<Test>>> GetResultCollection()
        {
            List<Tuple<Method, IEnumerable<Test>>> output = new List<Tuple<Method,IEnumerable<Test>>>();
            foreach (Method mtd in Methods)
            {
                IEnumerable<Test> testList =  TestRecords.SelectMany(tstr => tstr.Tests)
                                                        .Where(tst => tst.MethodID == mtd.ID);
                    
                output.Add(new Tuple<Method, IEnumerable<Test>>(mtd, testList));
            }

            return output;
        }
        
        /// <summary>
        /// Fetches the TestRecord entities for this ExternalReport
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TestRecord> GetTestRecords(bool includeTests = false)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<TestRecord> query = entities.TestRecords.Include(tstr => tstr.Batch.Material.Aspect)
                                                                    .Include(tstr => tstr.Batch.Material.MaterialLine)
                                                                    .Include(tstr => tstr.Batch.Material.MaterialType)
                                                                    .Include(tstr => tstr.Batch.Material.Recipe);

                if (includeTests)
                    query = query.Include(tstr => tstr.Tests
                                                      .Select(tst => tst.SubTests));

                return query.Where(tstr => tstr.ExternalReports
                            .Any(extr => extr.ID == ID))
                            .ToList();
            }
        }

        /// <summary>
        /// Removes a given method from the method associations and from every test record
        /// in the report
        /// </summary>
        /// <param name="methodEntity">The method that will be removed</param>
        public void RemoveTestMethod(Method methodEntity)
        {
            using (DBEntities entities = new DBEntities())
            {
                ExternalReport attachedExternalReport = entities.ExternalReports.First(ext => ext.ID == ID);
                Method attachedMethod = entities.Methods.First(mtd => mtd.ID == methodEntity.ID);

                attachedExternalReport.Methods.Remove(attachedMethod);

                IEnumerable<TestRecord> recordList = attachedExternalReport.TestRecords.ToList();

                IEnumerable<Test> testList = attachedExternalReport.TestRecords.SelectMany(tstr => tstr.Tests)
                                                                    .Where(tst => tst.MethodID == methodEntity.ID)
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
                                                    .Include(exrep => exrep.Methods
                                                    .Select(mtd => mtd.Property))
                                                    .Include(exrep => exrep.Methods
                                                    .Select(mtd => mtd.Standard))
                                                    .Include(exrep => exrep.Project.Leader)
                                                    .Include(exrep => exrep.Project.Oem)
                                                    .First(rep => rep.ID == entry.ID);

                entry.ArrivalDate = tempEntry.ArrivalDate;
                entry.Description = tempEntry.Description;
                entry.ExternalLab = tempEntry.ExternalLab;
                entry.ExternalLabID = tempEntry.ExternalLabID;
                entry.MaterialSent = tempEntry.MaterialSent;
                entry.Methods = tempEntry.Methods;
                entry.Project = tempEntry.Project;
                entry.ProjectID = tempEntry.ProjectID;
                entry.ReportReceived = tempEntry.ReportReceived;
                entry.RequestDone = tempEntry.RequestDone;
                entry.Samples = tempEntry.Samples;
            }
        }
               

    }
}
