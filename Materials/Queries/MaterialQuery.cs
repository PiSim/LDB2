using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query object that returns the first Material entry with a given AspectID, MaterialLineID, MaterialTypeID, RecipeID
    /// AspectID :  The AspectID that will be searched for
    /// MaterialLineID :  The MAterialLineID that will be searched for
    /// MaterialTypeID :  The MaterialTypeID that will be searched for
    /// RecipeID :  The RecipeID that will be searched for
    /// </summary>
    public class MaterialQuery : IScalar<Material, LabDbEntities>
    {
        #region Properties

        public int? AspectID { get; set; } = null;
        public int? MaterialLineID { get; set; } = null;
        public int? MaterialTypeID { get; set; } = null;
        public int? RecipeID { get; set; } = null;

        #endregion Properties

        #region Methods

        public Material Execute(LabDbEntities context)
        {
            return context.Materials.FirstOrDefault(mat => mat.AspectID == AspectID &&
                                                            mat.LineID == MaterialLineID &&
                                                            mat.TypeID == MaterialTypeID &&
                                                            mat.RecipeID == RecipeID);
        }

        #endregion Methods
    }
}