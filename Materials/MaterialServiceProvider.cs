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

        public Material CreateNewMaterial()
        {
            Material output;
            MaterialCreationDialog matDialog = _container.Resolve<MaterialCreationDialog>();
            
            if (matDialog.ShowDialog() == true)
            {
                output = _entities.Materials.First(mat => mat.ID == matDialog.ValidatedMaterial.ID);
                if (output.Construction.Project == null)
                {
                    ProjectPickerDialog prjDialog = new ProjectPickerDialog(_entities);
                    if (prjDialog.ShowDialog() == true)
                        output.Construction.Project = prjDialog.ProjectInstance;
                }

                if (output.Recipe.Colour == null)
                    output.Recipe.Colour = PickColourForRecipe();

                return output;
            }

            else
                return null;
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
