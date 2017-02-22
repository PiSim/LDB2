using Controls.Views;
using DBManager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches
{
    public class BatchServiceProvider
    {
        private DBEntities _entities;
        private UnityContainer _container;

        public BatchServiceProvider(DBEntities entities, UnityContainer container)
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
        
        public Batch GetBatch(string batchNumber)
        {
            Batch temp = _entities.GetBatchByNumber(batchNumber);

            if (temp.Material == null)
                temp.Material = CreateNewMaterial();

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
            
            return temp;
        }

        public Material CreateNewMaterial()
        {
            MaterialCreationDialog matDialog = new MaterialCreationDialog(_entities);
            if (matDialog.ShowDialog() == true)
                return matDialog.ValidatedMaterial;

            else
                return null;
        }
    }
}
