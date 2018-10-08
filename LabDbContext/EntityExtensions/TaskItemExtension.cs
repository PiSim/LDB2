using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public partial class TaskItem
    {
        #region Properties

        public string MethodName => Method?.Standard?.Name;

        public string PropertyName => Method?.Property?.Name;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a new Test entry based on the values currently loaded in the Task instance
        /// </summary>
        /// <returns>The new Test entity</returns>
        public Test GetTest()
        {
            Test output = new Test()
            {
                MethodVariantID = Method.MethodVariants.First().ID,
                Notes = Description,
                RequirementID = RequirementID
            };

            foreach (SubTaskItem subItem in SubTaskItems)
                output.SubTests.Add(subItem.GetSubTest());

            return output;
        }

        #endregion Methods
    }
}