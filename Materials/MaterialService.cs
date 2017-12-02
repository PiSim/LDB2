using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries.Presentation;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Events;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials
{
    public class MaterialService : IMaterialService
    {
        // Service class for internal use of the Materials Module
        // Performs various CRUD operations on Material-related entities
        
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private readonly IEnumerable<IQueryPresenter<Batch>> _batchQueries = new List<IQueryPresenter<Batch>>
        {
            new ArrivedUntestedBatchesQueryPresenter(),
            new BatchesNotArrivedQueryPresenter(),
            new Latest25BatchesQueryPresenter()
        };
        private IUnityContainer _container;

        public MaterialService(DBPrincipal principal,
                                EventAggregator eventAggregator,
                                IUnityContainer container)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _principal = principal;

            _eventAggregator.GetEvent<BatchVisualizationRequested>()
                            .Subscribe(batchNumber =>
                            {
                                TryQuickBatchVisualize(batchNumber);
                            });

            _eventAggregator.GetEvent<SampleCreationRequested>()
                            .Subscribe(tuple =>
                            {
                                AddSampleLog(tuple.Item1, tuple.Item2);
                            });


            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    Batch targetBatch = MaterialService.GetBatch(report.BatchID);

                    if (targetBatch.BasicReportID == null)
                    {
                        targetBatch.BasicReportID = report.ID;
                        targetBatch.Update();
                    }
                });
        }

        public Sample AddSampleLog(string batchNumber, string actionCode)
        {
            Batch temp = CommonProcedures.GetBatch(batchNumber);

            Sample output = new Sample();

            output.BatchID = temp.ID;
            output.Date = DateTime.Now;
            output.Code = actionCode;
            output.personID = _principal.CurrentPerson.ID;

            output.Create();

            if (actionCode == "A" && !temp.FirstSampleArrived)
            {
                temp.FirstSampleArrived = true;
                temp.FirstSampleID = output.ID;
                temp.Update();
            }

            return output;
        }

        public Aspect CreateAspect()
        {
            Views.AspectCreationDialog aspectCreationDialog = new Views.AspectCreationDialog();

            if (aspectCreationDialog.ShowDialog() == true)
            {
                aspectCreationDialog.AspectInstance.Create();
                return aspectCreationDialog.AspectInstance;
            }

            else
                return null;
        }


        internal static Batch CreateBatch()
        {
            Views.BatchCreationDialog batchCreator = new Views.BatchCreationDialog();

            if (batchCreator.ShowDialog() == true)
            {
                return batchCreator.BatchInstance;
            }

            else
                return null;
        }


        public ExternalConstruction CreateNewExternalConstruction()
        {
            ExternalConstruction newEntry = new ExternalConstruction();
            IEnumerable<ExternalConstruction> tempList = DBManager.Services.MaterialService.GetExternalConstructions();

            int nameCounter = 1;
            string curName = "Nuova Construction";
            while (true)
            {
                if (!tempList.Any(exc => exc.Name == curName))
                    break;

                else
                    curName = "Nuova Construction " + nameCounter++;
            }
            newEntry.Name = curName;
            newEntry.OemID = 1;

            newEntry.Create();

            return newEntry;
        }


        public void DeleteSample(Sample smp)
        {
            smp.Delete();
            SampleLogChoiceWrapper tempChoice = SampleLogActions.ActionList.First(scc => scc.Code == smp.Code);
            Batch tempBatch = MaterialService.GetBatch(smp.BatchID);

            tempBatch.ArchiveStock -= tempChoice.ArchiveModifier;
            tempBatch.LongTermStock -= tempChoice.LongTermModifier;
            tempBatch.Update();
        }

        public Batch GetBatch(string batchNumber)
        {
            Batch temp = MaterialService.GetBatch(batchNumber);

            if (temp == null)
            {
                temp = new Batch()
                {
                    Number = batchNumber
                };

                temp.Create();
            }

            return temp;
        }

        public IEnumerable<IQueryPresenter<Batch>> GetBatchQueries() => _batchQueries;

        private void OnColorCreationRequested()
        {
            Views.ColorCreationDialog colorCreator =  _container.Resolve<Views.ColorCreationDialog>();
            colorCreator.ShowDialog();
        }


        public static Batch StartBatchSelection()
        {
            BatchPickerDialog batchPicker = new BatchPickerDialog();
            if (batchPicker.ShowDialog() == true)
            {
                Batch output = GetBatch(batchPicker.BatchNumber);
                return output;
            }

            else
                return null;
        }

        public static void StartSampleLog()
        {
            SampleLogDialog logger = new SampleLogDialog();

            logger.Show();
        }

        public void TryQuickBatchVisualize(string batchNumber)
        {

            Batch temp = DBManager.Services.MaterialService.GetBatch(batchNumber);

            if (temp != null)
            {
                NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView, temp);
                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }

            else
                _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("Batch non trovato");
        }
    }
}
