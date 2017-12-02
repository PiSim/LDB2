using DBManager;
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

namespace Instruments
{
    public class InstrumentService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public InstrumentService(EventAggregator aggregator,
                                        IUnityContainer container)
        {
            _eventAggregator = aggregator;
            _container = container;

            _eventAggregator.GetEvent<InstrumentCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                RegisterNewInstrument();
                            });
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

        public IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> reqList)
        {
            List<TaskItem> output = new List<TaskItem>();

            foreach (Requirement req in reqList)
            {
                TaskItem tempItem = new TaskItem();

                tempItem.Description = req.Description;
                tempItem.MethodID = req.MethodID;
                tempItem.Name = req.Name;
                tempItem.Position = 0;
                tempItem.RequirementID = req.ID;
                tempItem.SpecificationVersionID = req.SpecificationVersionID;

                foreach (SubRequirement sreq in req.SubRequirements)
                {
                    SubTaskItem tempSubItem = new SubTaskItem();

                    tempSubItem.Name = sreq.SubMethod.Name;
                    tempSubItem.RequiredValue = sreq.RequiredValue;
                    tempSubItem.SubMethodID = sreq.SubMethodID;
                    tempSubItem.SubRequirementID = sreq.ID;
                    tempSubItem.UM = sreq.SubMethod.UM;

                    tempItem.SubTaskItems.Add(tempSubItem);
                }

                output.Add(tempItem);
            }

            return output;
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
        
        public static int GetNextCalibrationNumber(int year)
        {
            // Returns the next available calibration number for a given year

            using (DBEntities entities = new DBEntities())
            {
                return entities.CalibrationReports
                                .Where(crep => crep.Year == year)
                                .Max(crep => crep.Number) + 1;
            }
        }


        public CalibrationReport RegisterNewCalibration(Instrument target)
        {
            Views.NewCalibrationDialog calibrationDialog = new Views.NewCalibrationDialog();
            calibrationDialog.InstrumentInstance = target;
            if (calibrationDialog.ShowDialog() == true)
            {
                CalibrationReport output = calibrationDialog.ReportInstance;

                output.Instrument.UpdateCalibrationDueDate();
                output.Instrument.Update();

                return output;
            }
            else return null;
        }

        public Instrument RegisterNewInstrument()
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
