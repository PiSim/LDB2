﻿using System.Data.Entity;

namespace DataAccess
{
    public interface IScalar<out T, T2> where T2 : DbContext
    {
        #region Methods

        T Execute(T2 context);

        #endregion Methods
    }
}