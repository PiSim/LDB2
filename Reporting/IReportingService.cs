using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reporting
{
    public interface IReportingService
    {
        void PrintBatchReport(IEnumerable<Batch> batchList);
        void PrintReportDataSheet(Report reportEntry);
        void PrintTaskList(IEnumerable<Task> taskList);
    }
}
