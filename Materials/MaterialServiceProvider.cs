using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials
{
    public class MaterialServiceProvider : IMaterialServiceProvider
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

        private Material GetMaterial()
        {
            Material output = null;
            Views.MaterialCreationDialog matDialog = _container.Resolve<Views.MaterialCreationDialog>();
            
            if (matDialog.ShowDialog() == true)
            {
                Construction tempConstruction = _entities.Constructions.FirstOrDefault(con => con.Type.Code == matDialog.MaterialType &&
                                                                                            con.Line == matDialog.MaterialLine &&
                                                                                            con.Aspect.Code == matDialog.MaterialAspect);
                
                Recipe tempRecipe = _entities.Recipes.FirstOrDefault(rcp => rcp.Code == matDialog.MaterialRecipe);

                if (tempConstruction != null && tempRecipe != null)
                    output = _entities.Materials.FirstOrDefault(mat => mat.ConstructionID == tempConstruction.ID &&
                                                                                mat.RecipeID == tempRecipe.ID);

                else 
                {
                    if (tempConstruction == null)
                    {
                        tempConstruction = new Construction();
                        tempConstruction.Type = _entities.MaterialTypes.First(mty => mty.Code == matDialog.MaterialType);
                        tempConstruction.Line = matDialog.MaterialLine;
                        tempConstruction.Aspect = _entities.Aspects.FirstOrDefault(asp => asp.Code == matDialog.MaterialAspect);
                        
                        if (tempConstruction.Aspect == null)
                        {
                            Aspect tempAspect = new Aspect();
                            tempAspect.Code = matDialog.MaterialAspect;
                            tempAspect.Name = "";
                            tempConstruction.Aspect = tempAspect;
                        }
                    }

                    if (tempRecipe == null)
                    {
                        tempRecipe = new Recipe();
                        tempRecipe.Code = matDialog.MaterialRecipe;
                    };
                }
                
                if (output == null)
                {
                    output = new Material();
                    output.Construction = tempConstruction;
                    output.Recipe = tempRecipe;
                }
            }

            return output;
        }

        public void CheckMaterialData(Material target)
        {
            if (target.Construction.Project == null)
            {
                Views.ProjectPickerDialog prjDialog = _container.Resolve<Views.ProjectPickerDialog>();
                if (prjDialog.ShowDialog() == true)
                    target.Construction.Project = _entities.Projects.First(prj => prj.ID == prjDialog.ProjectInstance.ID);
            }

            if (target.Recipe.Colour == null)
            {
                Views.ColorPickerDialog colourPicker = _container.Resolve<Views.ColorPickerDialog>();
                if (colourPicker.ShowDialog() == true)
                    target.Recipe.Colour = _entities.Colours.First(clr => clr.ID == colourPicker.ColourInstance.ID);
            }
        }

        public Batch GetBatch(string batchNumber)
        {
            Batch temp = _entities.Batches.FirstOrDefault(bb => bb.Number == batchNumber);

            if (temp == null)
            {
                temp = new Batch();
                temp.Number = batchNumber;
                _entities.Batches.Add(temp);
            }

            if (temp.Material == null)
                temp.Material = GetMaterial();

            if (temp.Material != null)
                CheckMaterialData(temp.Material);
            
            _entities.SaveChanges();
            return temp;
        }

        private void OnColorCreationRequested()
        {
            Views.ColorCreationDialog colorCreator =  _container.Resolve<Views.ColorCreationDialog>();
            colorCreator.ShowDialog();
        }

        public Batch StartBatchSelection()
        {
            Views.BatchPickerDialog batchPicker = _container.Resolve<Views.BatchPickerDialog>();
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

            Batch temp = _entities.Batches.FirstOrDefault(btc => btc.Number == batchNumber);

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
