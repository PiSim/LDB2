using System.Collections;

namespace Infrastructure.Wrappers
{
    public interface ITestItem
    {
        #region Properties

        string MethodName { get; }
        string Notes { get; }
        string PropertyName { get; }
        IEnumerable SubItems { get; }
        double? WorkHours { get; }

        #endregion Properties
    }
}