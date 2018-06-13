using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Scripts
{
    public class BuildSubMethodPositionScript : ScriptBase
    {
        public BuildSubMethodPositionScript()
        {
            _name = "BuildSubMethodPositionScript";
        }

        public override void Run()
        {
            using (DBEntities entities = new DBEntities())
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
    }
    
}
