﻿using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MeasurableQuantityExtension
    {
        public static void Create(this MeasurableQuantity entry)
        {
            // Inserts a new MeasurableQuantity entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.MeasurableQuantities.Add(entry);
                entities.SaveChanges();
            }
        }
        
        public static void Delete(this MeasurableQuantity entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities
                        .MeasurableQuantities
                        .First(meq => meq.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<MeasurementUnit> GetMeasurementUnits(this MeasurableQuantity entry)
        {
            // Returns all MeasurementUnit for a given MeasurableQuantity

            if (entry == null)
                return new List<MeasurementUnit>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.Where(meu => meu.MeasurableQuantityID == entry.ID)
                                                .ToList();
            }
        }
    }
}
