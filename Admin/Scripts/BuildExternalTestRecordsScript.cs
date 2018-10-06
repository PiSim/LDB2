using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Scripts
{
    public class BuildExternalTestRecordsScript : ScriptBase
    {
        #region Constructors

        public BuildExternalTestRecordsScript()
        {
            _name = "BuildExternalTestRecords";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                IEnumerable<ExternalReport> exrepList = entities.ExternalReports.ToList();

                foreach (ExternalReport exrep in exrepList)
                {
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}