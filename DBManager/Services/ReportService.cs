using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ReportService
    {
        
        // TO BE MOVED IN SEPARATE CLASS

        public static List<Requirement> GenerateRequirementList(SpecificationVersion version)
        {
            if (version.IsMain)
                return new List<Requirement>(version.Requirements);

            else
            {
                List<Requirement> output = new List<Requirement>(
                    version.Specification.SpecificationVersions.First(sv => sv.IsMain).Requirements);

                foreach (Requirement requirement in version.Requirements)
                {
                    int ii = output.FindIndex(rr => rr.Method.ID == requirement.Method.ID);
                    output[ii] = requirement;
                }

                return output;
            }
        }

        #region Operations for ExternalConstruction entities

        public static Report GetReport(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Reports.First(entry => entry.ID == ID);
            }
        }

        public static void Create(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Reports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Reports.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;

                entities.Reports.Attach(entry);

                Report tempEntry = entities.Reports
                                            .Include(rep => rep.Batch)
                                            .Include(rep => rep.ParentTask)
                                            .Include(rep => rep.SpecificationIssues)
                                            .Include(rep => rep.Tests)
                                            .Include(rep => rep.SpecificationVersion)
                                            .First(rep => rep.ID == entryID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);

            }
        }

        public static void Update(this Report entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                Report tempEntry = entities.Reports.First(rep => rep.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion

    }
}
