using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    public class BatchesQuery : IQuery<Batch, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the query is returned as AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public string ConstructionName { get; set; }

        public int? ExternalConstructionID { get; set; }

        public string MaterialLineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string Notes { get; set; }

        public string OEMName { get; set; }

        public bool OrderResults { get; set; } = true;

        public string ProjectDescription { get; set; }
        public string ProjectNumber { get; set; }
        public string RecipeCode { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<Batch> Execute(LabDbEntities entities)
        {
            IQueryable<Batch> query = entities.Batches;

            if (!string.IsNullOrWhiteSpace(AspectCode))
                query = query.Where(btc => btc.Material.Aspect.Code.Contains(AspectCode));

            if (!string.IsNullOrWhiteSpace(BatchNumber))
                query = query.Where(btc => btc.Number.Contains(BatchNumber));

            if (!string.IsNullOrWhiteSpace(ColorName))
                query = query.Where(btc => btc.Material.Recipe.Colour.Name.Contains(ColorName));

            if (!string.IsNullOrWhiteSpace(ConstructionName))
                query = query.Where(btc => btc.Material.ExternalConstruction.Name.Contains(ConstructionName));

            if (ExternalConstructionID != null)
                query = query.Where(btc => btc.Material.ExternalConstructionID == ExternalConstructionID);

            if (!string.IsNullOrWhiteSpace(MaterialLineCode))
                query = query.Where(btc => btc.Material.MaterialLine.Code.Contains(MaterialLineCode));

            if (!string.IsNullOrWhiteSpace(MaterialTypeCode))
                query = query.Where(btc => btc.Material.MaterialType.Code.Contains(MaterialTypeCode));

            if (!string.IsNullOrWhiteSpace(Notes))
                query = query.Where(btc => btc.Notes.Contains(Notes));

            if (!string.IsNullOrWhiteSpace(OEMName))
                query = query.Where(btc => btc.Material.ExternalConstruction.Oem.Name.Contains(OEMName));

            if (!string.IsNullOrWhiteSpace(ProjectDescription))
                query = query.Where(btc => btc.Material.Project.Description.Contains(ProjectDescription));

            if (!string.IsNullOrWhiteSpace(ProjectNumber))
                query = query.Where(btc => btc.Material.Project.Name.Contains(ProjectNumber));

            if (!string.IsNullOrWhiteSpace(RecipeCode))
                query = query.Where(btc => btc.Material.Recipe.Code.Contains(RecipeCode));

            if (OrderResults)
                query = query.OrderByDescending(btc => btc.Number);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}