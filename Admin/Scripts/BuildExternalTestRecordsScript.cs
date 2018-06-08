using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Scripts
{
    public class BuildExternalTestRecordsScript : ScriptBase
    {
        public BuildExternalTestRecordsScript()
        {
            _name = "BuildExternalTestRecords";
        }

        public override void Run()
        {
            using (DBEntities entities = new DBEntities())
            {
                IEnumerable<ExternalReport> exrepList = entities.ExternalReports.ToList();

                foreach (ExternalReport exrep in exrepList)
                {

                }

                entities.SaveChanges();
            }
        }
    }
}
