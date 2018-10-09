using Infrastructure.Wrappers;
using LabDbContext;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IReportService
    {
        #region Methods

        bool AddTestsToReport(Report entry);

        void ApplyControlPlan(IEnumerable<ISelectableRequirement> requirementList, ControlPlan selectedControlPlan);

        ExternalReport CreateExternalReport();

        Report CreateReport();

        Requirement GenerateRequirement(MethodVariant mtd);

        IEnumerable<TaskItem> GenerateTaskItemList(IEnumerable<Requirement> requirementList);

        int GetNextExternalReportNumber(int year);

        int GetNextReportNumber();

        #endregion Methods
    }
}