using LabDbContext;
using System.Linq;

namespace Admin.Scripts
{
    public class BuildMethodVersionRequirementReferences : ScriptBase
    {
        #region Constructors

        public BuildMethodVersionRequirementReferences()
        {
            _name = "BuildMethodVersionRequirementReferences";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (Test test in entities.Tests.Where(tst => tst.MethodVariantID == null).ToList())
                {
                    test.MethodVariantID = test.Deprecated.MethodVariants.First().ID;
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}