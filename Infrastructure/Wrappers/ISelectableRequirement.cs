using LabDbContext;

namespace Infrastructure.Wrappers
{
    public interface ISelectableRequirement : ITestItem
    {
        #region Properties

        bool IsSelected { get; set; }
        Requirement RequirementInstance { get; }

        #endregion Properties
    }
}