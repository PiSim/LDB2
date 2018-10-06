using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class ProjectExtension
    {
        #region Methods

        public static IEnumerable<ExternalReport> GetExternalReports(this Project entry)
        {
            // Returns the external reports for a Project

            using (LabDbEntities entities = new LabDbEntities())
            {
                return entities.ExternalReports
                                .Where(erep => erep
                                .ProjectID == entry.ID);
            }
        }

        public static IEnumerable<Material> GetMaterials(this Project entry)
        {
            // Returns all Material entities for a Project

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.ExternalConstruction)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe.Colour)
                                        .Where(con => con.ProjectID == entry.ID)
                                        .ToList();
            }
        }

        public static IEnumerable<Test> GetTests(this Project entry)
        {
            // Returns all the tests for a project

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests
                                .Where(tst => tst
                                .TestRecord
                                .Batch
                                .Material
                                .ProjectID == entry.ID)
                                .ToList();
            }
        }

        #endregion Methods
    }

    public partial class Project
    {
        #region Properties

        public int BatchCount
        {
            get
            {
                using (LabDbEntities entities = new LabDbEntities())
                {
                    return entities.Batches
                                    .Where(btc => btc.Material.ProjectID == ID)
                                    .Count();
                }
            }
        }

        public int ExternalReportCount => ExternalReports.Count;

        public int MaterialCount
        {
            get
            {
                using (LabDbEntities entities = new LabDbEntities())
                {
                    return entities.Materials
                                    .Where(mat => mat.ProjectID == ID)
                                    .Count();
                }
            }
        }

        public string ProjectString => Name + " " + Description;

        public int ReportCount
        {
            get
            {
                using (LabDbEntities entities = new LabDbEntities())
                {
                    return entities.Reports
                                    .Where(rep => rep.Batch.Material.ProjectID == ID)
                                    .Count();
                }
            }
        }

        public IEnumerable<Report> Reports { get; private set; }

        #endregion Properties

        #region Methods

        public void Create()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Projects.Add(this);
                entities.SaveChanges();
            }
        }

        public void Delete()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                Project tempEntry = entities.Projects.FirstOrDefault(prj => prj.ID == ID);

                if (tempEntry != null)
                {
                    entities.Entry(tempEntry)
                            .State = EntityState.Deleted;

                    entities.SaveChanges();
                }

                entities.SaveChanges();
            }
        }

        public IEnumerable<Batch> GetBatches()
        {
            // Gets all batches for a given Project entity,
            // returns empty list if instance is null

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Where(btc => btc.Material.ProjectID == ID)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .ToList();
            }
        }

        /// <summary>
        /// Calculates the total external cost of the project and stores the result in the
        /// TotalExternalCost field
        /// </summary>
        /// <returns>The calculated value</returns>
        public double GetExternalReportCost()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                IQueryable<ExternalReport> externalReportList = entities.ExternalReports
                                                                            .Where(extr => extr.ProjectID == ID);

                return (externalReportList.Count() == 0) ? 0 : externalReportList.Sum(extr => extr.OrderTotal);
            }
        }

        /// <summary>
        /// CAlculates the internal cost of the project and stores the result in the
        /// TotalInternalCost field
        /// </summary>
        /// <returns>The calculated value</returns>
        public double GetInternalReportCost()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                IQueryable<Test> testList = entities.Tests
                                                    .Where(tst => tst.TestRecord.Batch.Material.ProjectID == ID);

                TotalReportDuration = (int)((testList.Count() == 0) ? 0 : testList.Sum(tst => tst.Duration));
            }

            return TotalReportDuration;
        }

        public IEnumerable<Report> GetReports()
        {
            // Returns all Report entities for the Project and stores the updated collection in the instance

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Reports = entities.Reports.Where(rep => rep.Batch.Material.ProjectID == ID)
                                                .Include(rep => rep.Author)
                                                .Include(rep => rep.Batch.Material.Aspect)
                                                .Include(rep => rep.Batch.Material.MaterialLine)
                                                .Include(rep => rep.Batch.Material.MaterialType)
                                                .Include(rep => rep.Batch.Material.Recipe.Colour)
                                                .Include(rep => rep.SpecificationVersion.Specification.Standard)
                                                .ToList();

                return Reports;
            }
        }

        public IEnumerable<Task> GetTasks()
        {
            // Returns all Task entities for the Project and stores the updated collection in the instance

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.Where(tsk => tsk.Batch.Material.ProjectID == ID)
                                    .Include(tsk => tsk.Batch.Material.Aspect)
                                    .Include(tsk => tsk.Batch.Material.MaterialLine)
                                    .Include(tsk => tsk.Batch.Material.MaterialType)
                                    .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                    .Include(tsk => tsk.Requester)
                                    .ToList();
            }
        }

        public void Load()
        {
            // Explicitly loads a Project and all related entities

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Project tempEntry = entities.Projects.Include(prj => prj.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(prj => prj.Leader)
                                                    .Include(prj => prj.Oem)
                                                    .First(prj => prj.ID == ID);

                Description = tempEntry.Description;
                ExternalReports = tempEntry.ExternalReports;
                Leader = tempEntry.Leader;
                Name = tempEntry.Name;
                Oem = tempEntry.Oem;
                OemID = tempEntry.OemID;
                ProjectLeaderID = tempEntry.ProjectLeaderID;
                TotalExternalCost = tempEntry.TotalExternalCost;
                TotalReportDuration = tempEntry.TotalReportDuration;
            }
        }

        public void Update()
        {
            // Updates the DBValues of the Project entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Projects
                        .AddOrUpdate(this);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}