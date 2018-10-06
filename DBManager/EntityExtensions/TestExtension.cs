﻿using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class TestExtension
    {
        #region Methods

        public static void CreateTests(this IEnumerable<Test> testList)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Tests.AddRange(testList);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Test entry)
        {
            // Deletes a Test entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Tests
                        .First(tst => tst.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static TaskItem GetTaskItem(this Test entry)
        {
            // Returns the task item that originated a Test entry, or null if one doesn't exist

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems.FirstOrDefault(tski => tski.TestID == entry.ID);
            }
        }

        public static void Update(this IEnumerable<Test> entryList)
        {
            // Updates all related Test and Subtest instances in a report

            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (Test tst in entryList)
                {
                    entities.Tests.AddOrUpdate(tst);
                    foreach (SubTest sts in tst.SubTests)
                        entities.SubTests.AddOrUpdate(sts);
                }
                entities.SaveChanges();
            }
        }

        #endregion Methods
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