using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class ExternalReportExtension
    {
        #region Methods

        public static IEnumerable<ExternalReportFile> GetExternalReportFiles(this ExternalReport entry)
        {
            // Returns all the files for an external report entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReportFiles.Where(extf => extf.ExternalReportID == entry.ID)
                                .ToList();
            }
        }

        #endregion Methods
    }

    public partial class ExternalReport
    {
        #region Proprietà

        public string FormattedNumber => Year.ToString() + Number.ToString("d3");

        #endregion Proprietà

        #region Metodi

        public void AddBatch(Batch batchEntity)
        {
            // Adds a Batch to an ExternalReport entry

            using (LabDbEntities entities = new LabDbEntities())
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
            using (LabDbEntities entities = new LabDbEntities())
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

        /// <summary>
        /// Returns a list of test for each methodVariant associated with the Report
        /// generated from the entity collections loaded in the instance
        /// </summary>
        /// <returns>An IEnumerable of Tuples where Value1 is a methodVAriant and
        /// Value2 is an IEnumerable of tests</returns>
        public IEnumerable<Tuple<MethodVariant, IEnumerable<Test>>> GetResultCollection()
        {
            List<Tuple<MethodVariant, IEnumerable<Test>>> output = new List<Tuple<MethodVariant, IEnumerable<Test>>>();
            foreach (MethodVariant mtdvar in MethodVariants)
            {
                IEnumerable<Test> testList = TestRecords.SelectMany(tstr => tstr.Tests)
                                                        .Where(tst => tst.MethodVariantID == mtdvar.ID);

                output.Add(new Tuple<MethodVariant, IEnumerable<Test>>(mtdvar, testList));
            }

            return output;
        }

        /// <summary>
        /// Removes a given methodVariant from the method associations and from every test record
        /// in the report
        /// </summary>
        /// <param name="methodEntity">The methodVariant that will be removed</param>
        public void RemoveTestMethodVariant(MethodVariant methodVariantEntity)
        {
            using (LabDbEntities entities = new LabDbEntities())
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
        

        #endregion Metodi
    }
}