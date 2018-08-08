using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class TestQuery : IQuery<Test>
    {
        public TestQuery()
        {

        }

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public bool IncludeExternalReports { get; set; }

        public bool IncludeInternalReports { get; set; }

        public string LineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string MethodName { get; set; }

        public string RecipeCode { get; set; }

        public IQueryable<Test> RunQuery(DBEntities entities)
        {
            IQueryable<Test> query = entities.Tests.Include(tst => tst.TestRecord.Batch.Material.Aspect)
                                                    .Include(tst => tst.TestRecord.Batch.Material.MaterialLine)
                                                    .Include(tst => tst.TestRecord.Batch.Material.MaterialType)
                                                    .Include(tst => tst.TestRecord.Batch.Material.Recipe)
                                                    .Include(tst => tst.TestRecord.ExternalReports)
                                                    .Include(tst => tst.TestRecord.Reports)
                                                    .Include(tst => tst.Method.Property)
                                                    .Include(tst => tst.Method.Standard)
                                                    .Include(tst => tst.SubTests);

            if (!IncludeExternalReports)
                query = query.Where(tst => tst.TestRecord.RecordTypeID != 2);

            if (!IncludeInternalReports)
                query = query.Where(tst => tst.TestRecord.RecordTypeID != 1);

            if (!string.IsNullOrWhiteSpace(AspectCode))
                query = query.Where(tst => tst.TestRecord.Batch.Material.Aspect.Code.Contains(AspectCode));
            
            if (!string.IsNullOrWhiteSpace(BatchNumber))
                query = query.Where(tst => tst.TestRecord.Batch.Number.Contains(BatchNumber));

            if (!string.IsNullOrWhiteSpace(ColorName))
                query = query.Where(tst => tst.TestRecord.Batch.Material.Recipe.Colour.Name.Contains(ColorName));

            if (!string.IsNullOrWhiteSpace(LineCode))
                query = query.Where(tst => tst.TestRecord.Batch.Material.MaterialLine.Code.Contains(LineCode));

            if (!string.IsNullOrWhiteSpace(MaterialTypeCode))
                query = query.Where(tst => tst.TestRecord.Batch.Material.MaterialType.Code.Contains(MaterialTypeCode));

            if (!string.IsNullOrWhiteSpace(MethodName))
                query = query.Where(tst => tst.Method.Standard.Name.Contains(MethodName));

            if (!string.IsNullOrWhiteSpace(RecipeCode))
                query = query.Where(tst => tst.TestRecord.Batch.Material.Recipe.Code.Contains(RecipeCode));

            if (!string.IsNullOrWhiteSpace(TestName))
                query = query.Where(tst => tst.Method.Property.Name.Contains(TestName));

            return query;
        }

        public string TestName { get; set; }
    }
}
