using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class ControlPlanItemB
    {
        public void Create()
        {
            // Inserts the Instance in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlanItemBs.Add(this);

                entities.SaveChanges();
            }
        }
    }
}
