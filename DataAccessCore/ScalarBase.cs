using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessCore
{
    public class ScalarBase<T, T2> : IScalar<T, T2> where T2 : DbContext
    {
        #region Properties

        public bool AsNoTracking { get; set; } = true;

        public bool EagerLoadingEnabled { get; set; } = true;

        public bool LazyLoadingDisabled { get; set; } = true;

        #endregion Properties

        #region Methods

        public virtual T Execute(T2 context)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}