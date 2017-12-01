using DBManager;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Infrastructure.Wrappers
{
    public class TaskItemWrapper : ITestItem
    {
        private TaskItem _instance;

        public TaskItemWrapper(TaskItem instance)
        {
            _instance = instance;
        }

        public string PropertyName => _instance?.Method?.Property?.Name;

        public string MethodName => _instance?.Method?.Standard?.Name;

        public string Notes => _instance?.Description;

        public IEnumerable SubItems => _instance?.SubTaskItems;

        public double? WorkHours => _instance?.WorkHours;
    }
}
