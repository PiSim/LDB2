using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class SubTaskItem
    {
        public int ID { get; set; }
        public int TaskItemID { get; set; }
        public string RequiredValue { get; set; }
        public Nullable<int> SubMethodID { get; set; }
        public string Name { get; set; }
        public string UM { get; set; }
        public Nullable<int> SubRequirementID { get; set; }

        public virtual SubMethod SubMethod { get; set; }
        public virtual SubRequirement SubRequirement { get; set; }
        public virtual TaskItem ParentTaskItem { get; set; }
    }
}