using DataAccessCore;
using Infrastructure.Events;
using Instruments.Commands;
using LInst;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;

namespace Instruments
{
    public class InstrumentService
    {
        #region Fields

        private IDesignTimeDbContextFactory<LInstContext> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LInstContext> _lInstData;

        #endregion Fields

        #region Constructors

        public InstrumentService(IDesignTimeDbContextFactory<LInstContext> dbContextFactory,
                            IEventAggregator aggregator,
                            IDataService<LInstContext> lInstData)
        {
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
            _lInstData = lInstData;
        }

        #endregion Constructors

        #region Methods

        public void AddCalibrationFile(IEnumerable<CalibrationFile> fileList)
        {
            // inserts a set of CalibrationFile entries in the DB

            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[] { }))
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

            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[] { }))
            {
                return entities.Instruments.Include(ins => ins.InstrumentType)
                                            .Include(ins => ins.UtilizationArea)
                                            .Include(ins => ins.CalibrationResponsible)
                                            .Where(ins => ins.IsUnderControl == true)
                                            .OrderBy(ins => ins.CalibrationDueDate)
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
            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[] { }))
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

                _lInstData.Execute(new UpdateInstrumentCalibrationStatusCommand(target));

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