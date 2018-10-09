using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.Queries
{
    /// <summary>
    /// Query object that returns  multiple Recipe entities
    /// </summary>
    public class RecipesQuery : QueryBase<Recipe, LabDbEntities>
    {
        /// <summary>
        /// Gets or Sets a ColorID to use as filter
        /// </summary>
        public int? ColorID { get; set; }

        public override IQueryable<Recipe> Execute(LabDbEntities context)
        {
            IQueryable<Recipe> query = context.Recipes;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (ColorID != null)
                query = query.Where(rec => rec.ColourID == ColorID);

            if (OrderResults)
                query = query.OrderBy(rec => rec.Code);

            return query;
        }
    }
}
