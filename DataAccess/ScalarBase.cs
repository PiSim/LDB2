using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ScalarBase<T, T2> : IScalar<T, T2> where T2: DbContext
    {
        public bool AsNoTracking { get; set; } = true;

        public bool EagerLoadingEnabled { get; set; } = true;

        public bool LazyLoadingDisabled { get; set; } = true;

        public virtual T Execute(T2 context)
        {
            throw new NotImplementedException();
        }
    }
}
