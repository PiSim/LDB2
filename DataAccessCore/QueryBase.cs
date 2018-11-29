using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessCore
{
    public class QueryBase<T, T2> : IQuery<T, T2> where T2 : DbContext
    {
        /// <summary>
        /// If true the query is executed AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the query will include the relevant subentities
        /// </summary>
        public bool EagerLoadingEnabled { get; set; } = true;


        /// <summary>
        /// If true Lazy loading will be disabled in the configuration
        /// </summary>
        public bool LazyLoadingDisabled { get; set; } = true;

        /// <summary>
        /// If true the results will be sorted
        /// </summary>
        public bool OrderResults { get; set; } = true;

        public virtual IQueryable<T> Execute(T2 context)
        {
            throw new NotImplementedException();
        }
    }
}
