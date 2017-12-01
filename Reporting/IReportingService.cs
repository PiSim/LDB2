using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reporting
{
    public interface IReportingService
    {
        void PrintBatchReport(IEnumerable<Batch> batchList);
        void PrintTaskList(IEnumerable<Task> taskList);
    }
}
