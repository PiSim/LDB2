using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public static class AspectExtension
    {
        #region Methods

        public static void Create(this Aspect entry)
        {
            if (entry == null)
                throw new NullReferenceException();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Aspects.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Aspect entry)
        {
            // Deletes an aspect entity

            if (entry == null)
                throw new NullReferenceException();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Aspects.First(asp => asp.ID == entry.ID))
                        .State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Update(this Aspect entry)
        {
            // Updates a given Aspect entry

            if (entry == null)
                throw new NullReferenceException();

            using (LabDbEntities entities = new LabDbEntities())
            {
                Aspect tempEntry = entities.Aspects.First(asp => asp.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

}