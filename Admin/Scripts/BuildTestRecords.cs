using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Scripts
{
    public class BuildTestRecords : ScriptBase
    {
        public BuildTestRecords()
        {
            _name = "BuildTestRecordsScript";
        }

        public override void Run()
        {
            using (DBEntities entities = new DBEntities())
            {
                IEnumerable<Test> testList = entities.Tests.ToList();

                foreach (Test tst in testList)
                {
                    tst.TestRecordID = tst.TBD2.TestRecordID;
                }
                entities.SaveChanges();
            }
        }
    }
}
