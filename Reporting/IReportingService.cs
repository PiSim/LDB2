using LabDbContext;
using System.Collections.Generic;

namespace Reporting
{
    public interface IReportingService
    {
        #region Methods

        void PrintBatchReport(IEnumerable<Batch> batchList);

        void PrintReportDataSheet(Report reportEntry);

        void PrintTaskList(IEnumerable<Task> taskList);

        #endregion Methods
    }
}