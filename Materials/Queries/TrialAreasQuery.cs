using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    public class TrialAreasQuery : IQuery<TrialArea, LabDbEntities>
    {
        #region Methods

        public IQueryable<TrialArea> Execute(LabDbEntities context)
        {
            return context.TrialAreas.Where(tra => true);
        }

        #endregion Methods
    }
}