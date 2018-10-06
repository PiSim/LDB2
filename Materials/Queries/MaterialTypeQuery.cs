using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns the First MaterialType entry with a given Code
    /// MaterialTypeCode : the Code to search for
    /// </summary>
    public class MaterialTypeQuery : IScalar<MaterialType, LabDbEntities>
    {
        #region Properties

        public string MaterialTypeCode { get; set; }

        #endregion Properties

        #region Methods

        public MaterialType Execute(LabDbEntities context)
        {
            return context.MaterialTypes.FirstOrDefault(mtyp => mtyp.Code == MaterialTypeCode);
        }

        #endregion Methods
    }
}