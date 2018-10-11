using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns a single SpecificationVersion entry
    /// </summary>
    public class SpecificationVersionQuery : IScalar<SpecificationVersion, LabDbEntities>
    {
        #region Properties

        public int? ID { get; set; }

        #endregion Properties

        #region Methods

        public SpecificationVersion Execute(LabDbEntities context)
        {
            IQueryable<SpecificationVersion> query = context.SpecificationVersions;

            return query.FirstOrDefault(specv => specv.ID == ID);
        }

        #endregion Methods
    }
}