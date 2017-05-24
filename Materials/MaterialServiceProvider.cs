using Controls.Views;
using DBManager;
using DBManager.Services;
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

namespace Materials
{
    public class MaterialServiceProvider
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public MaterialServiceProvider(DBEntities entities,
                                        DBPrincipal principal,
                                        EventAggregator eventAggregator,
                                        IUnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;


        }

        public Sample AddSampleLog(string batchNumber, string actionCode)
        {
            Batch temp = GetBatch(batchNumber);

            Sample output = new Sample();

            output.Batch = temp;
            output.Date = DateTime.Now;
            output.Code = actionCode;
            output.LogAuthor = _entities.People.First(ppl => ppl.ID == _principal.CurrentPerson.ID);

            _entities.Samples.Add(output);
            _entities.SaveChanges();

            return output;
        }

        public static ExternalConstruction CreateNewExternalConstruction()
        {
            ExternalConstruction newEntry = new ExternalConstruction();
            IEnumerable<ExternalConstruction> tempList = MaterialService.GetExternalConstructions();

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

            newEntry.Create();

            return newEntry;
        }

        private static Material GetMaterial()
        {
            Material output = null;
            Views.MaterialCreationDialog matDialog = new Views.MaterialCreationDialog();
            
            if (matDialog.ShowDialog() == true)
            {
                Construction tempConstruction = MaterialService.GetConstruction(matDialog.MaterialType,
                                                                                matDialog.MaterialLine,
                                                                                matDialog.MaterialAspect);
                
                Recipe tempRecipe = MaterialService.GetRecipe(matDialog.MaterialRecipe);

                if (tempConstruction != null && tempRecipe != null)
                    output = MaterialService.GetMaterial(tempConstruction, tempRecipe);

                else
                {
                    if (tempConstruction == null)
                    {
                        tempConstruction = new Construction();
                        tempConstruction.Type = MaterialService.GetMaterialType(matDialog.MaterialType);
                        tempConstruction.Line = matDialog.MaterialLine;
                        tempConstruction.Aspect = MaterialService.GetAspect(matDialog.MaterialAspect);

                        if (tempConstruction.Aspect == null)
                        {
                            Aspect tempAspect = new Aspect();
                            tempAspect.Code = matDialog.MaterialAspect;
                            tempAspect.Name = "";

                            tempAspect.Create();

                            tempConstruction.Aspect = tempAspect;
                        }


                        tempConstruction.Update();
                    }

                    if (tempRecipe == null)
                    {
                        tempRecipe = new Recipe();
                        tempRecipe.Code = matDialog.MaterialRecipe;

                        tempRecipe.Create();
                    };

                    output.Update();
                }
                
                if (output == null)
                {
                    output = new Material();
                    output.Construction = tempConstruction;
                    output.Recipe = tempRecipe;

                    output.Create();
                }
            }

            return output;
        }

        public static void CheckMaterialData(Material target)
        {
            if (target.Construction.Project == null)
            {
                Views.ProjectPickerDialog prjDialog = new Views.ProjectPickerDialog();
                if (prjDialog.ShowDialog() == true)
                    target.Construction.Project = prjDialog.ProjectInstance;
            }

            if (target.Recipe.Colour == null)
            {
                Views.ColorPickerDialog colourPicker = new Views.ColorPickerDialog();
                if (colourPicker.ShowDialog() == true)
                    target.Recipe.Colour = colourPicker.ColourInstance;
            }
        }

        public static Batch GetBatch(string batchNumber)
        {
            Batch temp = MaterialService.GetBatch(batchNumber);

            if (temp == null)
            {
                temp = new Batch();
                temp.Number = batchNumber;
                temp.Create();
            }

            if (temp.Material == null)
                temp.Material = GetMaterial();

            if (temp.Material != null)
                CheckMaterialData(temp.Material);
            
            return temp;
        }

        private void OnColorCreationRequested()
        {
            Views.ColorCreationDialog colorCreator =  _container.Resolve<Views.ColorCreationDialog>();
            colorCreator.ShowDialog();
        }

        public static Batch StartBatchSelection()
        {
            Views.BatchPickerDialog batchPicker = new Views.BatchPickerDialog();
            if (batchPicker.ShowDialog() == true)
            {
                Batch output = GetBatch(batchPicker.BatchNumber);
                return output;
            }

            else
                return null;
        }

        public void TryQuickBatchVisualize(string batchNumber)
        {

            Batch temp = MaterialService.GetBatch(batchNumber);

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
