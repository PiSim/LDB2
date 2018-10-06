using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Scripts
{
    public class BuildSubMethodPositionScript : ScriptBase
    {
        #region Constructors

        public BuildSubMethodPositionScript()
        {
            _name = "BuildSubMethodPositionScript";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                IEnumerable<Method> methodList = entities.Methods.ToList();

                foreach (Method mtd in methodList)
                {
                    int positionCounter = 0;

                    foreach (SubMethod smtd in mtd.SubMethods)
                    {
                        smtd.Position = positionCounter++;
                    }
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}