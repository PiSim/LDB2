using DataAccess;
using LabDbContext;
using System.Linq;

namespace Specifications.Queries
{
    /// <summary>
    /// Query Object that returns a single Std entity
    /// </summary>
    public class StandardQuery : IScalar<Std, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a Name string to search for
        /// </summary>
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public Std Execute(LabDbEntities context)
        {
            IQueryable<Std> query = context.Stds;

            return query.FirstOrDefault(std => std.Name == Name);
        }

        #endregion Methods
    }
}