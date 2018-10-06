using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.Commands
{
    public class BatchInsertUpdateBase : ICommand<LabDbEntities>
    {
        internal LabDbEntities _context;
        
        public virtual void Execute(LabDbEntities context)
        {
            throw new NotImplementedException();
        }
               
        /// <summary>
        /// Creates a new Material with the type, line, aspect and recipe codes stored in the Command Instance in the given context
        /// Looks for existing Entities and creates new ones if they don't exist
        /// </summary>
        /// <param name="template"> A disconnected material entry containing the informations required to create the new material</param>
        /// <returns>A new connected Material entry</returns>
        internal Material CreateMaterial(Material template)
        {
            /// Instantiate new Material

            Material newMaterial = new Material();
            _context.Materials.Add(newMaterial);

            // Retrieves the MaterialType entity with the given code and sets reference in the new material
            // If no such entity exists, throws exception

            MaterialType typeInstance = _context.MaterialTypes.FirstOrDefault(mtyp => mtyp.Code == template.MaterialType.Code);

            newMaterial.MaterialType = typeInstance ?? throw new InvalidOperationException("MaterialType with code " + template.MaterialType.Code + " does not exist.");

            // Check if a MaterialLine instance with the given code exists
            // If no such entity exists, throws exception

            MaterialLine lineInstance = _context.MaterialLines.FirstOrDefault(mlin => mlin.Code == template.MaterialLine.Code);

            newMaterial.MaterialLine = lineInstance ?? throw new InvalidOperationException("MaterialLine with code " + template.MaterialLine.Code + " does not exist.");

            // Check if an Aspect instance with the given code exists
            // If no such entity exists, throws exception

            Aspect aspectInstance = _context.Aspects.FirstOrDefault(asp => asp.Code == template.Aspect.Code);

            newMaterial.Aspect = aspectInstance ?? throw new InvalidOperationException("Aspect with code " + template.Aspect.Code + " does not exist.");

            // Check if a Recipe instance with the given code exists
            // If no such entity exists, throws exception

            Recipe connectedrecipe = _context.Recipes.FirstOrDefault(rec => rec.Code == template.Recipe.Code);

            newMaterial.Recipe = connectedrecipe ?? throw new InvalidOperationException("Recipe with code " + template.Recipe.Code + " does not exist.");


            newMaterial.ExternalConstructionID = template.ExternalConstruction?.ID;
            newMaterial.ProjectID = template.Project?.ID;
            newMaterial.Recipe.ColourID = template.Recipe?.Colour?.ID;

            return newMaterial;
        }

        /// <summary>
        /// Checks the required fields of the template instance for inconsistencies and throws an exception if any is found
        /// </summary>
        /// <param name="templateInstance"></param>
        internal virtual void ValidateMaterialTemplate(Material templateInstance)
        {
            if (templateInstance == null)
                throw new InvalidOperationException("A Material Template instance is required");

            if (templateInstance.Aspect == null)
                throw new InvalidOperationException("Invalid Material Template: Aspect is a required Field");

            if (templateInstance.MaterialLine == null)
                throw new InvalidOperationException("Invalid Material Template: MaterialLine is a required Field");

            if (templateInstance.MaterialType == null)
                throw new InvalidOperationException("Invalid Material Template: MaterialType is a required Field");

            if (templateInstance.Recipe == null)
                throw new InvalidOperationException("Invalid Material Template: Recipe is a required Field");
        }

        /// <summary>
        /// Searches the db for a Material entry matching the provided template and creates a new one if none is found
        /// </summary>
        /// <param name="template">A material entry tto use as template for the query and the creation</param>
        /// <returns></returns>
        internal virtual Material GetOrCreateMaterial(Material template)
        {
            Material _connectedMaterial = _context.Materials.FirstOrDefault(mat => mat.Aspect.Code == template.Aspect.Code &&
                                                                mat.MaterialLine.Code == template.MaterialLine.Code &&
                                                                mat.MaterialType.Code == template.MaterialType.Code &&
                                                                mat.Recipe.Code == template.Recipe.Code);

            // If a material with the given parameters does not exist, create it

            if (_connectedMaterial == null)
                _connectedMaterial = CreateMaterial(template);

            else
            {
                if (template.ExternalConstruction != null)
                    _connectedMaterial.ExternalConstructionID = template.ExternalConstruction.ID;

                if (template.Project != null)
                    _connectedMaterial.ProjectID = template.Project.ID;

                if (template.Recipe.Colour != null)
                    _connectedMaterial.Recipe.ColourID = template.Recipe.Colour.ID;
            }

            return _connectedMaterial;
        }
    }
}
