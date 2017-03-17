using Controls.Views;
using DBManager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials
{
    public class MaterialServiceProvider
    {
        private DBEntities _entities;
        private UnityContainer _container;

        public MaterialServiceProvider(DBEntities entities,
                                    UnityContainer container)
        {
            _container = container;
            _entities = entities;
        }

        public Sample AddSampleLog(string batchNumber, string actionCode)
        {
            Batch temp = GetBatch(batchNumber);

            Sample output = new Sample();

            output.Batch = temp;
            output.Date = DateTime.Now;
            output.Code = actionCode;

            _entities.Samples.Add(output);
            _entities.SaveChanges();

            return output;
        }

        private Material GetMaterial()
        {
            if (target.Material != null)
                return;

            MaterialCreationDialog matDialog = _container.Resolve<MaterialCreationDialog>();
            
            if (matDialog.ShowDialog() == true)
            {
                Construction tempConstruction = _entities.Constructions(con => con.Type.Code matDialog.MaterialType &&
                                                                                con.Line == matDialog.MaterialLine &&
                                                                                con.Aspect.Code == matDialog.MaterialAspect);
                
                Recipe tempRecipe = _entities.Recipes(rcp => rcp.Code == matDialog.MaterialRecipe);

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
                    }

                    if (tempRecipe == null)
                    {
                        tempRecipe = new Recipe();
                        tempRecipe.Code == matDialog.MaterialRecipe;
                    };
                }
                
                if (output == null)
                {
                    output = new Material();
                    output.Construction = tempConstruction;
                    output.Recipe = tempRecipe;
                }

                return output;
            }

            else
                return;
        }

        public void CheckMaterialData(Material target)
        {
            if (output.Construction.Project == null)
            {
                ProjectPickerDialog prjDialog = new ProjectPickerDialog(_entities);
                if (prjDialog.ShowDialog() == true)
                    output.Construction.Project = prjDialog.ProjectInstance;
            }

            if (output.Recipe.Colour == null)
                output.Recipe.Colour = PickColourForRecipe();

        }

        public Batch GetBatch(string batchNumber)
        {
            Batch temp = Batches.FirstOrDefault(bb => bb.Number == batchNumber);

            if (temp == null)
            {
                output = new Batch();
                output.Number = batchNumber;
            }

            if (temp.Material != null)
            {
                if (temp.Material.Construction.Project == null)
                {
                    ProjectPickerDialog prjDialog = new ProjectPickerDialog(_entities);
                    if (prjDialog.ShowDialog() == true)
                        temp.Material.Construction.Project = prjDialog.ProjectInstance;
                }
                if (temp.Material.Recipe.Colour == null)
                {
                    ColorPickerDialog colourPicker = new ColorPickerDialog(_entities);
                    if (colourPicker.ShowDialog() == true)
                        temp.Material.Recipe.Colour = colourPicker.ColourInstance;
                }
            }

            if (temp.Material == null)
                temp.Material = CreateNewMaterial();

            return temp;
        }

        private void OnColorCreationRequested()
        {
            ColorCreationDialog colorCreator =  _container.Resolve<ColorCreationDialog>();
            colorCreator.ShowDialog();
        }

        public Colour PickColourForRecipe()
        {
            ColorPickerDialog colourPicker = new ColorPickerDialog(_entities);

            if (colourPicker.ShowDialog() == true)
                return colourPicker.ColourInstance;

            else
                return null;
        } 
    }
}
