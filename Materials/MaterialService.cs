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
        private IDataService _dataService;
        private readonly IEnumerable<IQueryPresenter<Batch>> _batchQueries = new List<IQueryPresenter<Batch>>
        {
            new ArrivedUntestedBatchesQueryPresenter(),
            new BatchesNotArrivedQueryPresenter(),
            new Latest25BatchesQueryPresenter()
        };
        private IUnityContainer _container;

        public MaterialService(DBPrincipal principal,
                                EventAggregator eventAggregator,
                                IDataService dataService,
                                IUnityContainer container)
        {
            _container = container;
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _principal = principal;

            _eventAggregator.GetEvent<BatchVisualizationRequested>()
                            .Subscribe(batchNumber =>
                            {
                                TryQuickBatchVisualize(batchNumber);
                            });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    Batch targetBatch = _dataService.GetBatch(report.BatchID);

                    if (targetBatch.BasicReportID == null)
                    {
                        targetBatch.BasicReportID = report.ID;
                        targetBatch.Update();
                    }
                });
        }

        public Sample AddSampleLog(string batchNumber, string actionCode)
        {
            Batch temp = _dataService.GetBatch(batchNumber);

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


        public Batch CreateBatch()
        {
            Views.BatchCreationDialog batchCreator = new Views.BatchCreationDialog();

            if (batchCreator.ShowDialog() == true)
            {
                EntityChangedToken token = new EntityChangedToken(batchCreator.BatchInstance,
                                                                    EntityChangedToken.EntityChangedAction.Created);
                _eventAggregator.GetEvent<BatchChanged>()
                                .Publish(token);
                return batchCreator.BatchInstance;
            }

            else
                return null;
        }

        public Material CreateMaterial(MaterialType typeInstance,
                                        MaterialLine lineInstance,
                                        Aspect aspectInstance,
                                        Recipe recipeInstance)
        {
            if (typeInstance.ID == 0 ||
                lineInstance.ID == 0 ||
                aspectInstance.ID == 0 ||
                recipeInstance.ID == 0)
                throw new InvalidOperationException();

            Material output = new Material();

            output.TypeID = typeInstance.ID;
            output.LineID = lineInstance.ID;
            output.AspectID = aspectInstance.ID;
            output.RecipeID = recipeInstance.ID;

            output.Create();

            return output;
        }
        
        public Material CreateMaterial(string typeCode,
                                        string lineCode,
                                        string aspectCode,
                                        string colorRecipeCode)
        {

            // Check if a MaterialType instance with the given code exists
            // If not, throw exception

            MaterialType typeInstance = _dataService.GetMaterialType(typeCode);

            if (typeInstance == null)
                throw new InvalidOperationException();

            // Check if a MaterialLine instance with the given code exists
            // If not, create it

            MaterialLine lineInstance = _dataService.GetMaterialLine(lineCode);

            if (lineInstance == null)
            {
                lineInstance = new MaterialLine()
                {
                    Code = lineCode
                };

                lineInstance.Create();
            }

            // Check if an Aspect instance with the given code exists
            // If not, create it

            Aspect aspectInstance = _dataService.GetAspect(aspectCode);

            if (aspectInstance == null)
            {
                aspectInstance = new Aspect()
                {
                    Code = aspectCode
                };

                aspectInstance.Create();
            }


            // Check if a Recipe instance with the given code exists
            // If not, create it

            Recipe recipeInstance = _dataService.GetRecipe(colorRecipeCode);

            if (recipeInstance == null)
            {
                recipeInstance = new Recipe()
                {
                    Code = colorRecipeCode
                };

                recipeInstance.Create();
            }

            // Call method to create Material instance with the gathered subEntities

            Material output = CreateMaterial(typeInstance,
                                            lineInstance,
                                            aspectInstance,
                                            recipeInstance);

            return output;
        }

        public ExternalConstruction CreateNewExternalConstruction()
        {
            ExternalConstruction newEntry = new ExternalConstruction();
            IEnumerable<ExternalConstruction> tempList = _dataService.GetExternalConstructions();

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
            Batch tempBatch = _dataService.GetBatch(smp.BatchID);

            tempBatch.ArchiveStock -= tempChoice.ArchiveModifier;
            tempBatch.LongTermStock -= tempChoice.LongTermModifier;
            tempBatch.Update();
        }

        public IEnumerable<IQueryPresenter<Batch>> GetBatchQueries() => _batchQueries;
        
        public Batch StartBatchSelection()
        {
            BatchPickerDialog batchPicker = new BatchPickerDialog();
            if (batchPicker.ShowDialog() == true)
            {
                Batch output = _dataService.GetBatch(batchPicker.BatchNumber);
                return output;
            }

            else
                return null;
        }

        public void ShowSampleLogDialog()
        {
            Views.SampleLogDialog logger = new Views.SampleLogDialog();

            logger.Show();
        }

        public void TryQuickBatchVisualize(string batchNumber)
        {

            Batch temp = _dataService.GetBatch(batchNumber);

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
