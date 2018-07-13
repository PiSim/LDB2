using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class BatchQuery : IQuery<Batch>
    {
        public BatchQuery()
        {
            OrderByNumberDescending = true;
        }

        public string AspectCode { get; set; }

        public string BatchNumber { get; set; }

        public string ColorName { get; set; }

        public string ConstructionName { get; set; }

        public string MaterialLineCode { get; set; }

        public string MaterialTypeCode { get; set; }

        public string Notes { get; set; }

        public string OEMName { get; set; }

        public bool OrderByNumberDescending { get; set; }

        public string ProjectNumber { get; set; }

        public string ProjectDescription { get; set; }

        public string RecipeCode { get; set; }

        public IQueryable<Batch> RunQuery(DBEntities entities)
        {
            IQueryable<Batch> query = entities.Batches.Include(btc => btc.BasicReport)
                                                    .Include(btc => btc.FirstSample)
                                                    .Include(btc => btc.LatestSample)
                                                    .Include(btc => btc.Material.Aspect)
                                                    .Include(btc => btc.Material.ExternalConstruction.Oem)
                                                    .Include(btc => btc.Material.MaterialLine)
                                                    .Include(btc => btc.Material.MaterialType)
                                                    .Include(btc => btc.Material.Project.Oem)
                                                    .Include(btc => btc.Material.Recipe.Colour)
                                                    .Include(btc => btc.TrialArea);

            if (!string.IsNullOrWhiteSpace(AspectCode))
                query = query.Where(btc => btc.Material.Aspect.Code.Contains(AspectCode));

            if (!string.IsNullOrWhiteSpace(BatchNumber))
                query = query.Where(btc => btc.Number.Contains(BatchNumber));

            if (!string.IsNullOrWhiteSpace(ColorName))
                query = query.Where(btc => btc.Material.Recipe.Colour.Name.Contains(ColorName));

            if (!string.IsNullOrWhiteSpace(ConstructionName))
                query = query.Where(btc => btc.Material.ExternalConstruction.Name.Contains(ConstructionName));

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

            if (OrderByNumberDescending)
                query = query.OrderByDescending(btc => btc.Number);

            return query;
        }
    }
}
