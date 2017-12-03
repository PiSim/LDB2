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
        // Repository for Common DB-Access methods -- DEPRECATED
        

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
    }
}
