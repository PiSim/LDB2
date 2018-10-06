using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query entity that returns the first MaterialLine entry with a given code
    /// MaterialLineCode : the code to look for
    /// </summary>
    public class MaterialLineQuery : IScalar<MaterialLine, LabDbEntities>
    {
        #region Properties

        public string MaterialLineCode { get; set; }

        #endregion Properties

        #region Methods

        public MaterialLine Execute(LabDbEntities context)
        {
            return context.MaterialLines.FirstOrDefault(matl => matl.Code == MaterialLineCode);
        }

        #endregion Methods
    }
}