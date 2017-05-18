using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class TaskItemWrapper
    {
        private bool _isSelected;
        private TaskItem _taskItemInstance;

        public TaskItemWrapper(TaskItem taskItemInstance)
        {
            _taskItemInstance = taskItemInstance;
            _isSelected = true;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public string Method
        {
            get { return _taskItemInstance.Requirement.Method.Standard.Name; }
        }

        public string Property
        {
            get { return _taskItemInstance.Requirement.Method.Property.Name; }
        }

        public Requirement RequirementInstance
        {
            get { return _taskItemInstance.Requirement; }
        }

        public TaskItem TaskItemInstance
        {
            get { return _taskItemInstance; }
        }
    }
}
