using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Instruments
{
    public class InstrumentService : IInstrumentService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public InstrumentService(EventAggregator aggregator,
                                        IUnityContainer container)
        {
            _eventAggregator = aggregator;
            _container = container;

        }


        public void AddCalibrationFiles(IEnumerable<CalibrationFiles> fileList)
        {
            // inserts a set of CalibrationFiles entries in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.CalibrationFiles.AddRange(fileList);
                entities.SaveChanges();
            }
        }

        public IEnumerable<Instrument> GetCalibrationCalendar()
        {
            // Returns a list of the instruments under control, ordered by due calibration date

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationResults
                                .ToList();
            }
        }

        /// <summary>
        /// Retrieves and returns the next available CalibrationReport number for a given year
        /// </summary>
        /// <param name="year">The year on which the query is performed</param>
        /// <returns>The first unused calibration number</returns>
        public static int GetNextCalibrationNumber(int year)
        {
            using (DBEntities entities = new DBEntities())
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

                output.Instrument.UpdateCalibrationDueDate();
                output.Instrument.Update();

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

        public Instrument CreateInstrument()
        {
            Views.InstrumentCreationDialog creationDialog = _container.Resolve<Views.InstrumentCreationDialog>();
            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Publish();
                return creationDialog.InstrumentInstance;
            }
            else
                return null;
        }
    }
}
