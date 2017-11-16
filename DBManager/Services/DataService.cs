using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class DataService
    {
        // Repository for Common DB-Access methods

        public static IEnumerable<Batch> GetBatchesForExternalConstruction(ExternalConstruction target,
                                                                    bool lazyLoadingEnabled = false)
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
                return entities.Batches.Where(btc => btc.Material.ExternalConstruction.ID == target.ID)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .ToList();
            }
        }

        public static IEnumerable<Material> GetMaterialsWithoutConstruction()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Materials.Where(mat => mat.ExternalConstruction == null)
                                            .Include(mat => mat.Aspect)
                                            .Include(mat => mat.MaterialLine)
                                            .Include(mat => mat.MaterialType)
                                            .Include(mat => mat.Recipe)
                                            .ToList();
            }
        }

        public static IEnumerable<Material> GetMaterialsWithoutProject()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Materials.Where(mat => mat.Project == null)
                                            .Include(mat => mat.Aspect)
                                            .Include(mat => mat.ExternalConstruction)
                                            .Include(mat => mat.MaterialLine)
                                            .Include(mat => mat.MaterialType)
                                            .Include(mat => mat.Recipe)
                                            .ToList();
            }
        }

        public static IEnumerable<OrganizationRole> GetOrganizationRoles()
        {
            //Returns all organization roles

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoles
                                .ToList();
            }
        }

        public static IEnumerable<Property> GetProperties()
        {
            // Returns all Property entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Properties.Where(prp => true)
                                            .ToList();
            }
        }

        public static IEnumerable<User> GetUsers()
        {
            // Returns all User entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Users.ToList();
            }
        }
    }
}
