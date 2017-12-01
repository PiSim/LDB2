using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Collections;

namespace DBManager
{
    public interface IDataService
    {
        IEnumerable<Batch> GetBatches();
        IEnumerable<Batch> GetBatches(int numberOfEntries);
        IList<T> GetQueryResults<T>(IQuery<T> query);
    }
}
