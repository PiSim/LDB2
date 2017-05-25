using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentExtension
    {
        public static void Create(this Instrument entry)
        {
            // Inserts a new Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                Instrument newEntry = new Instrument();

                entities.Instruments.Add(newEntry);
                entities.Entry(newEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();

                entry.ID = newEntry.ID;
            }
        }
    }
}
