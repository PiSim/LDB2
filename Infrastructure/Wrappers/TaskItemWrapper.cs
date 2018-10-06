using LabDbContext;
using System.Collections;

namespace Infrastructure.Wrappers
{
    public class TaskItemWrapper : ITestItem
    {
        #region Fields

        private TaskItem _instance;

        #endregion Fields

        #region Constructors

        public TaskItemWrapper(TaskItem instance)
        {
            _instance = instance;
        }

        #endregion Constructors

        #region Properties

        public string MethodName => _instance?.Method?.Standard?.Name;
        public string Notes => _instance?.Description;
        public string PropertyName => _instance?.Method?.Property?.Name;
        public IEnumerable SubItems => _instance?.SubTaskItems;

        public double? WorkHours => _instance?.WorkHours;

        #endregion Properties
    }
}