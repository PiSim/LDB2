using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Scripts
{
    public class BuildTestRecords : ScriptBase
    {
        #region Constructors

        public BuildTestRecords()
        {
            _name = "BuildTestRecordsScript";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                IEnumerable<Test> testList = entities.Tests.ToList();

                foreach (Test tst in testList)
                {
                    tst.TestRecordID = tst.TBD2.TestRecordID;
                }
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}