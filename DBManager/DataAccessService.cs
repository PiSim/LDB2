using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace LabDbContext
{
    public class DataAccessService : IDataService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _dbContextFactory;

        #endregion Fields

        #region Constructors

        public DataAccessService(IDbContextFactory<LabDbEntities> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        #region Methods

        public CalibrationReport GetCalibrationReport(int ID)
        {
            // Returns a calibration report with the given ID, or null if none is found

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports.FirstOrDefault(calrep => calrep.ID == ID);
            }
        }
        
        public IEnumerable<ControlPlan> GetControlPlans()
        {
            using (var entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.ControlPlans.Include(cpl => cpl.Specification)
                                            .OrderBy(cpl => cpl.Specification.Standard.Name)
                                            .ToList();
            }
        }
        
        public IEnumerable<MeasurableQuantity> GetMeasurableQuantities()
        {
            // Returns all Measurable Quantities

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.ToList();
            }
        }

        public IEnumerable<MeasurementUnit> GetMeasurementUnits()
        {
            // Returns all measurement units

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.ToList();
            }
        }

        /// <summary>
        /// Returns all OrganizationRoles
        /// </summary>
        /// <returns>An IEnumerable containing all the OrganizationRole entities</returns>
        public IEnumerable<OrganizationRole> GetOrganizationRoles()
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoles
                                .ToList();
            }
        }

        /// <summary>
        /// Returns all PersonRole Entities
        /// </summary>
        /// <returns>An IEnumerable containing all PersonRole entities</returns>
        public IEnumerable<PersonRole> GetPersonRoles()
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PersonRoles.ToList();
            }
        }
        
        public Requirement GetRequirement(int ID)
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.First(req => req.ID == ID);
            }
        }


        /// <summary>
        /// Returns all Task entities in the DB
        /// </summary>
        /// <param name="includeComplete">If true complete tasks will be included</param>
        /// <param name="includeAssigned">If true assigned tasks will be included</param>
        /// <returns>IEnumerable containing all the found entries</returns>
        public IEnumerable<Task> GetTasks(bool includeComplete = true,
                                        bool includeAssigned = true)
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<Task> queryBase = entities.Tasks.Include(tsk => tsk.Batch.Material.Aspect)
                                                            .Include(tsk => tsk.Batch.Material.Project)
                                                            .Include(tsk => tsk.Batch.Material.MaterialLine)
                                                            .Include(tsk => tsk.Batch.Material.MaterialType)
                                                            .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                            .Include(tsk => tsk.Requester)
                                                            .Include(tsk => tsk.SpecificationVersion.Specification.Standard);

                if (includeComplete)
                    return queryBase.ToList();

                if (includeAssigned)
                    return queryBase.Where(tsk => tsk.Report == null || !tsk.Report.IsComplete)
                                    .ToList();
                else
                    return queryBase.Where(tsk => tsk.Report == null)
                                    .ToList();
            }
        }

        public IEnumerable<User> GetUsers()
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Users.ToList();
            }
        }

        #endregion Methods
    }
}