using LabDbContext;
using System.Linq;

namespace Admin.Scripts
{
    public class BuildExternalReportMethodVariantMappingScript : ScriptBase
    {
        #region Constructors

        public BuildExternalReportMethodVariantMappingScript()
        {
            _name = "BuildExternalReportMethodVariantMappingScript";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (ExternalReport exrep in entities.ExternalReports.ToList())
                {
                    foreach (Method mtd in exrep.Deprecated)
                    {
                        exrep.MethodVariants.Add(mtd.MethodVariants.First());
                    }
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}