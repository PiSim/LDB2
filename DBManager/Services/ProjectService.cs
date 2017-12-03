using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ProjectService
    {
        #region Operations for Project entitites


        public static Project GetProject(int ID)
        {
            // Returns a single unloaded Project instance given its ID

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.First(entry => entry.ID == ID);
            }
        }

        public static Project GetProject(string name)
        {
            // Returns a Project with the given name or null if none exists

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.FirstOrDefault(prj => prj.Name == name);
            }
        }

        #endregion  

    }
}
