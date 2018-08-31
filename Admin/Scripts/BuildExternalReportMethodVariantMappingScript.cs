using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Scripts
{
    public class BuildExternalReportMethodVariantMappingScript : ScriptBase
    {
        public BuildExternalReportMethodVariantMappingScript()
        {
            _name = "BuildExternalReportMethodVariantMappingScript";
        }

        public override void Run()
        {
            using (DBEntities entities = new DBEntities())
            {
                foreach(ExternalReport exrep in entities.ExternalReports.ToList())
                {
                    foreach ( Method mtd in exrep.Deprecated)
                    {
                        exrep.MethodVariants.Add(mtd.MethodVariants.First());
                    }
                }

                entities.SaveChanges();
            }
        }
    }
}
