using DBManager;
using Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public interface IReportService
    {
        bool AddTestsToReport(Report entry);
        void ApplyControlPlan(IEnumerable<ISelectableRequirement> requirementList, ControlPlan selectedControlPlan);
        ExternalReport CreateExternalReport();
        Report CreateReport();
        Report CreateReport(Task parentTask);
        IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> requirementList);
        int GetNextExternalReportNumber(int year);
        int GetNextReportNumber();
        Requirement GenerateRequirement(MethodVariant mtd);
    }
}
