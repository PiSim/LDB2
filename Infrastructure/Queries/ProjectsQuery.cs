using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query Object that returns all Projects
    /// IncludeCollections (def. false) : If true all children collections (Reports, Batches, materials) are included in the query
    /// OrderResults (def. true) : If true the results are ordered by projectName descending
    /// </summary>
    public class ProjectsQuery : IQuery<Project, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If True the query is returned AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        public bool IncludeCollections { get; set; } = false;

        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Project> Execute(LabDbEntities context)
        {
            IQueryable<Project> query = context.Projects.Include(prj => prj.Leader)
                                                .Include(prj => prj.Oem);

            if (IncludeCollections)
                query = query.Include(prj => prj.ExternalReports)
                                                .Include(prj => prj.Materials
                                                .Select(mat => mat.Batches
                                                .Select(btc => btc.Reports)));

            if (OrderResults)
                query = query.OrderByDescending(prj => prj.Name);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}