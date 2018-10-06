using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns all Color entities
    /// SortResults (def. true) : if true the results are ordered by name
    /// </summary>
    public class ColorsQuery : IQuery<Colour, LabDbEntities>
    {
        #region Properties

        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Colour> Execute(LabDbEntities context)
        {
            IQueryable<Colour> query = context.Colours;

            if (OrderResults)
                query = query.OrderBy(col => col.Name);

            return query;
        }

        #endregion Methods
    }
}