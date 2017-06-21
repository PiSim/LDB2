using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MaterialTypeExtension
    {
        public static void Create(this MaterialType entry)
        {
            // Inserts a MaterialType entry

            using (DBEntities entities = new DBEntities())
            {
                entities.MaterialTypes.Add(entry);
                entities.SaveChanges();
            }
        }
    }
}
