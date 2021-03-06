﻿using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    public class TestQuery : IQuery<Test, LabDbEntities>
    {
        #region Constructors

        public TestQuery()
        {
        }

        #endregion Constructors

        #region Properties

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public bool IncludeExternalReports { get; set; }

        public bool IncludeInternalReports { get; set; }

        public string LineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string MethodName { get; set; }

        public string RecipeCode { get; set; }

        public string TestName { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<Test> Execute(LabDbEntities entities)
        {
            IQueryable<Test> query = entities.Tests.Include(tst => tst.TestRecord.Batch.Material.Aspect)
                                                    .Include(tst => tst.TestRecord.Batch.Material.MaterialLine)
                                                    .Include(tst => tst.TestRecord.Batch.Material.MaterialType)
                                                    .Include(tst => tst.TestRecord.Batch.Material.Recipe)
                                                    .Include(tst => tst.TestRecord.ExternalReports)
                                                    .Include(tst => tst.TestRecord.Reports)
                                                    .Include(tst => tst.MethodVariant.Method.Property)
                                                    .Include(tst => tst.MethodVariant.Method.Standard)
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
                query = query.Where(tst => tst.MethodVariant.Method.Standard.Name.Contains(MethodName));

            if (!string.IsNullOrWhiteSpace(RecipeCode))
                query = query.Where(tst => tst.TestRecord.Batch.Material.Recipe.Code.Contains(RecipeCode));

            if (!string.IsNullOrWhiteSpace(TestName))
                query = query.Where(tst => tst.MethodVariant.Method.Property.Name.Contains(TestName));

            return query;
        }

        #endregion Methods
    }
}