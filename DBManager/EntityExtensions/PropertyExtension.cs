using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Property
    {
        /// <summary>
        /// Inserts the entity in the DB as a new entry
        /// </summary>
        public void Create()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Properties.Add(this);
                entities.SaveChanges();
            }
        }
    }
}
