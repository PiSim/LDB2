using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ControlPlanItem
    {
        public int ControlPlanID { get; set; }
        public int RequirementID { get; set; }
        public bool IsSelected { get; set; }
        public int ID { get; set; }

        public virtual ControlPlan ControlPlan { get; set; }
        public virtual Requirement Requirement { get; set; }
    }
}