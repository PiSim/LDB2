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
        private UnityContainer _container;

        public MaterialServiceProvider(UnityContainer container)
        {
            _container = container;
        }

        public Material CreateNewMaterial()
        {
            Material output;
            MaterialCreationDialog matDialog = _container.Resolve<MaterialCreationDialog>();
            if (matDialog.ShowDialog() == true)
            {
                output = matDialog.ValidatedMaterial;
                if (output.Construction.Project == null)
                {
                    ProjectPickerDialog prjDialog = _container.Resolve<ProjectPickerDialog>();
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

        public Colour PickColourForRecipe()
        {
            ColorPickerDialog colourPicker = _container.Resolve<ColorPickerDialog>();

            if (colourPicker.ShowDialog() == true)
                return colourPicker.ColourInstance;

            else
                return null;
        } 
    }
}
