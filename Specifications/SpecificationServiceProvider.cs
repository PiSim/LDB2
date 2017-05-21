﻿using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    public class SpecificationServiceProvider : ISpecificationServiceProvider
    {
        private DBEntities _entities;
        private UnityContainer _container;

        public SpecificationServiceProvider(DBEntities entities,
                                            UnityContainer container)
        {
            _entities = entities;
            _container = container;
        }


        #region Operations for Specification entities

        public Specification GetSpecification(int ID)
        {
            using (DBEntities entities = new DBEntities())
            { 
                return entities.Specifications.First(entry => entry.ID == ID);
            }
        }

        public void CreateSpecification(Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public void DeleteSpecification(Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public void LoadSpecification(ref Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;

                entry = entities.Specifications.Include(spec => spec.SpecificationVersions)
                                                .Include(spec => spec.Standard)
                                                .Include(spec => spec.Standard.CurrentIssue)
                                                .First(spec => spec.ID == entryID);   
            }
        }

        public void UpdateSpecification(Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion
    }
}
