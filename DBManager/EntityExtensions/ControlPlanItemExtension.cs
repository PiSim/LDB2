using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class ControlPlanItem
    {
        public void Create()
        {
            // Inserts the Instance in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlanItems.Add(this);

                entities.SaveChanges();
            }
        }
    }
}
