using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Commands
{
    /// <summary>
    /// Command entity that given a mocked Material instance checks if an equivalent entry exists in the database.
    /// If not, a new entry is created and inserted.
    /// </summary>
    public class CreateMaterialIfMissingCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private string _aspectCode,
                    _lineCode,
                    _recipeCode,
                    _typeCode;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base Constructor for the command
        /// </summary>
        /// <param name="material">A mocked Material instance containing instances of Aspect, Line, Type and Recipe entries
        /// with codes existing in the database</param>
        public CreateMaterialIfMissingCommand(Material material)
        {
            _aspectCode = material.Aspect.Code;
            _lineCode = material.MaterialLine.Code;
            _typeCode = material.MaterialType.Code;
            _recipeCode = material.Recipe.Code;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            if (!context.Materials.Any(mat => mat.Aspect.Code == _aspectCode &&
                                            mat.MaterialLine.Code == _lineCode &&
                                            mat.Recipe.Code == _recipeCode &&
                                            mat.MaterialType.Code == _typeCode))
            {
                Aspect aspectInstance = context.Aspects.First(asp => asp.Code == _aspectCode);
                MaterialLine lineInstance = context.MaterialLines.First(lin => lin.Code == _lineCode);
                MaterialType typeInstance = context.MaterialTypes.First(typ => typ.Code == _typeCode);
                Recipe recipeInstance = context.Recipes.First(rec => rec.Code == _recipeCode);

                context.Materials.Add(new Material()
                {
                    Aspect = aspectInstance,
                    MaterialLine = lineInstance,
                    MaterialType = typeInstance,
                    Recipe = recipeInstance
                });

                context.SaveChanges();
            }
        }

        #endregion Methods
    }
}