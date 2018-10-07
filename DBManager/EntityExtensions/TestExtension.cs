using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class TestExtension
    {

        public static void CreateTests(this IEnumerable<Test> testList)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Tests.AddRange(testList);
                entities.SaveChanges();
            }
        }
        
    }

    public partial class Test
    {
        #region Properties

        public string AspectCode => TestRecord?.Batch?.Material?.Aspect?.Code;

        public string BatchNumber => TestRecord?.Batch?.Number;

        public string MaterialLineCode => TestRecord?.Batch?.Material?.MaterialLine?.Code;

        public string MaterialTypeCode => TestRecord?.Batch?.Material?.MaterialType?.Code;

        public string MethodName => MethodVariant?.Method?.Standard?.Name;

        public string PropertyName => MethodVariant?.Method?.Property?.Name;

        public string RecipeCode => TestRecord?.Batch?.Material?.Recipe?.Code;

        public string ReportNumber
        {
            get
            {
                if (TestRecord.RecordTypeID == 1)
                    return "TR" +
                            TestRecord.Reports.FirstOrDefault()?.Number.ToString();
                else if (TestRecord.RecordTypeID == 2)
                    return "TE" +
                            TestRecord.ExternalReports.FirstOrDefault()?.Year.ToString() +
                            TestRecord.ExternalReports.FirstOrDefault()?.Number.ToString("d3");
                else return "";
            }
        }

        #endregion Properties
    }
}