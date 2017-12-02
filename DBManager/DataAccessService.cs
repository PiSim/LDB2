using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DataAccessService : IDataService
    {

        public DataAccessService()
        {

        }
        
        public IEnumerable<Batch> GetBatches()
        {
            // Returns all Batches

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .Where(btc => true)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public IEnumerable<Batch> GetBatches(int numberOfEntries)
        {
            // Returns the first numberOfEntries Batches by number descending

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .OrderByDescending(btc => btc.Number)
                                        .Take(numberOfEntries)
                                        .ToList();
            }
        }


        public IEnumerable<CalibrationResult> GetCalibrationResults()
        {
            // Returns all CalibrationResult entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationResults
                                .ToList();
            }
        }


        public CalibrationReport GetCalibrationReport(int ID)
        {
            // Returns a calibration report with the given ID, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports.FirstOrDefault(calrep => calrep.ID == ID);
            }
        }

        public IEnumerable<CalibrationReport> GetCalibrationReports()
        {
            // Returns all Calibrationreport entities, ordered by number descending

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Include(crep => crep.Instrument)
                                .Include(crep => crep.Laboratory)
                                .Include(crep => crep.CalibrationResult)
                                .Include(crep => crep.Tech)
                                .OrderByDescending(crep => crep.Year)
                                .ThenByDescending(crep => crep.Number)
                                .ToList();
            }
        }
        
        public static Instrument GetInstrument(string code)
        {
            // Returns the instrument entry with the given code, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.FirstOrDefault(inst => inst.Code == code);
            }
        }

        public IEnumerable<Instrument> GetInstruments()
        {
            // Returns all Instrument entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.Include(inst => inst.InstrumentType)
                                            .Include(inst => inst.Manufacturer)
                                            .Include(inst => inst.InstrumentUtilizationArea)
                                            .OrderBy(inst => inst.Code)
                                            .ToList();
            }
        }


        public IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            // Returns all InstrumentType entities

            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentTypes
                                .OrderBy(insty => insty.Name)
                                .ToList();
            }
        }


        public IEnumerable<MeasurableQuantity> GetMeasurableQuantities()
        {
            // Returns all Measurable Quantities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.ToList();
            }
        }
        public static IEnumerable<MeasurementUnit> GetMeasurementUnits()
        {
            // Returns all measurement units

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.ToList();
            }
        }

        public IEnumerable<Person> GetPeople(string roleName = null)
        {
            // Returns all People entities, a rolename can be provided to filter by

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.People.Where(per => roleName == null || per.RoleMappings
                                        .FirstOrDefault(prm => prm.Role.Name == roleName)
                                        .IsSelected)
                                        .OrderBy(per => per.Name)
                                        .ToList();
            }
        }

        /// <summary>
        /// Returns all PersonRole Entities
        /// </summary>
        /// <returns>An IEnumerable containing all PersonRole entities</returns>
        public IEnumerable<PersonRole> GetPersonRoles()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PersonRoles.ToList();
            }
        }

        public IList<T> GetQueryResults<T>(IQuery<T> query)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return query.RunQuery(entities)
                            .ToList();
            }
        }

        /// <summary>
        /// Returns the TaskItem with the given ID
        /// </summary>
        /// <param name="ID">The ID to look up</param>
        /// <returns>The task item with the given ID, or null if none is found</returns>
        public TaskItem GetTaskItem(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems.FirstOrDefault(tski => tski.ID == ID);
            }
        }

        /// <summary>
        /// Returns the task entry with the given ID
        /// </summary>
        /// <param name="ID">the ID to look up</param>
        /// <returns>The task entry with the given ID, or null if none is found</returns>
        public Task GetTask(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.FirstOrDefault(tsk => tsk.ID == ID);
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
            using (DBEntities entities = new DBEntities())
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


        public IEnumerable<InstrumentUtilizationArea> GetUtilizationAreas()
        {

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentUtilizationAreas
                                .OrderBy(iua => iua.Name)
                                .ToList();
            }
        }
    }
}
