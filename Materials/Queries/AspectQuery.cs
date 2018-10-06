using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns the First Aspect entry with a given Code
    /// AspectCode : the code to search for
    /// </summary>
    public class AspectQuery : IScalar<Aspect, LabDbEntities>
    {
        #region Properties

        public string AspectCode { get; set; }

        #endregion Properties

        #region Methods

        public Aspect Execute(LabDbEntities context)
        {
            return context.Aspects.FirstOrDefault(asp => asp.Code == AspectCode);
        }

        #endregion Methods
    }
}