using DataAccess;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Events;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Instruments
{
    public class InstrumentService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public InstrumentService(IDbContextFactory<LabDbEntities> dbContextFactory,
                            IEventAggregator aggregator,
                            IDataService<LabDbEntities> labDbData)
        {
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
            _labDbData = labDbData;
        }

        #endregion Constructors

        #region Methods

        public void AddCalibrationFiles(IEnumerable<CalibrationFiles> fileList)
        {
            // inserts a set of CalibrationFiles entries in the DB

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.CalibrationFiles.AddRange(fileList);
                entities.SaveChanges();
            }
        }

        public Instrument CreateInstrument()
        {
            Views.InstrumentCreationDialog creationDialog = new Views.InstrumentCreationDialog();
            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Publish();
                return creationDialog.InstrumentInstance;
            }
            else
                return null;
        }

        public IEnumerable<Instrument> GetCalibrationCalendar()
        {
            // Returns a list of the instruments under control, ordered by due calibration date

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.Include(ins => ins.InstrumentType)
                                            .Include(ins => ins.InstrumentUtilizationArea)
                                            .Include(ins => ins.CalibrationResponsible)
                                            .Where(ins => ins.IsUnderControl == true)
                                            .OrderBy(ins => ins.CalibrationDueDate)
                                            .ToList();
            }
        }

        public IEnumerable<CalibrationResult> GetCalibrationResults()
        {
            // Returns all CalibrationResult entities

            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationResults
                                .AsNoTracking()
                                .ToList();
            }
        }

        /// <summary>
        /// Retrieves and returns the next available CalibrationReport number for a given year
        /// </summary>
        /// <param name="year">The year on which the query is performed</param>
        /// <returns>The first unused calibration number</returns>
        public int GetNextCalibrationNumber(int year)
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
            {
                try
                {
                    return entities.CalibrationReports
                                    .Where(crep => crep.Year == year)
                                    .Max(crep => crep.Number) + 1;
                }
                catch
                {
                    return 1;
                }
            }
        }

        public CalibrationReport ShowNewCalibrationDialog(Instrument target)
        {
            Views.NewCalibrationDialog calibrationDialog = new Views.NewCalibrationDialog
            {
                InstrumentInstance = target
            };

            if (calibrationDialog.ShowDialog() == true)
            {
                CalibrationReport output = calibrationDialog.ReportInstance;

                target.UpdateCalibrationDueDate();
                _labDbData.Execute(new UpdateEntityCommand(target));

                _eventAggregator.GetEvent<CalibrationIssued>()
                                .Publish(output);

                return output;
            }
            else return null;
        }

        public InstrumentMaintenanceEvent ShowNewMaintenanceDialog(Instrument entry)
        {
            Views.MaintenanceEventCreationDialog maintenanceEventCreationDialog = new Views.MaintenanceEventCreationDialog
            {
                InstrumentInstance = entry
            };

            if (maintenanceEventCreationDialog.ShowDialog() == true)
            {
                return maintenanceEventCreationDialog.InstrumentEventInstance;
            }
            else
                return null;
        }

        #endregion Methods
    }
}