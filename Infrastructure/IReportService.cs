using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public interface IReportService
    {
        bool AddTestsToReport(Report entry);
        ExternalReport CreateExternalReport();
        Report CreateReport();
        Report CreateReport(Task parentTask);
    }
}
