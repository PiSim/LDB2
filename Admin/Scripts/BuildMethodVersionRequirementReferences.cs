using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Scripts
{
    public class BuildMethodVersionRequirementReferences : ScriptBase
    {
        public BuildMethodVersionRequirementReferences()
        {
            _name = "BuildMethodVersionRequirementReferences";
        }

        public override void Run()
        {
            using (DBEntities entities = new DBEntities())
            {
                foreach (Test test in entities.Tests.Where(tst => tst.MethodVariantID == null).ToList())
                {
                    test.MethodVariantID = test.Deprecated.MethodVariants.First().ID;
                }

                entities.SaveChanges();
            }
        }
    }
}
