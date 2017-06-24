using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MaterialLineExtension
    {
        public static void Create(this MaterialLine entry)
        {
            // Inserts a new MaterialLine entry

            using (DBEntities entitites = new DBEntities())
            {
                entitites.MaterialLines.Add(entry);

                entitites.SaveChanges();
            }
        }
    }
}
