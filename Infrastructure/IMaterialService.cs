using DBManager;
using Infrastructure.Queries.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IMaterialService
    {
        Aspect CreateAspect();
        
        /// <summary>
        /// Creates a new Material instance with the given subEntities and inserts
        /// it into the Database
        /// </summary>
        /// <param name="typeInstance">An existing MaterialType Instance</param>
        /// <param name="lineInstance">An existing MaterialLine Instance</param>
        /// <param name="aspectInstance">An existing Aspect Instance</param>
        /// <param name="recipeInstance">An existing Recipe Instance</param>
        /// <returns>The newly created Material instance</returns>
        Material CreateMaterial(MaterialType typeInstance,
                                MaterialLine lineInstance,
                                Aspect aspectInstance,
                                Recipe recipeInstance);

        /// <summary>
        /// Creates a new Material instance with the given parameters and inserts
        /// it into the database
        /// </summary>
        /// <param name="typeCode">A string representing the MaterialType</param>
        /// <param name="lineCode">A string representing the MaterialLine</param>
        /// <param name="aspectCode">A string representing the Aspect</param>
        /// <param name="colorRecipeCode">A string representing the Recipe</param>
        /// <returns>The newly created Material instance</returns>
        Material CreateMaterial(string typeCode,
                                string lineCode,
                                string aspectCode,
                                string colorRecipeCode);
        
        IEnumerable<IQueryPresenter<Batch>> GetBatchQueries();
        void DeleteSample(Sample smp);
        Batch StartBatchSelection();
        ExternalConstruction CreateNewExternalConstruction();
        void ShowSampleLogDialog();
        Batch CreateBatch();
    }
}
