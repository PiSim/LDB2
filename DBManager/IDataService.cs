
using System.Collections.Generic;

namespace DBManager
{
    public interface IDataService
    {
        IEnumerable<Batch> GetBatches();
        IEnumerable<Batch> GetBatches(int numberOfEntries);
    }
}
