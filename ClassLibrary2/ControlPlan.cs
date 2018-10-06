using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class ControlPlan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ControlPlan()
        {
            this.control_plan_items_b = new HashSet<ControlPlanItem>();
        }

        public int ID { get; set; }
        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public int SpecificationID { get; set; }

        public virtual Specification Specification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ControlPlanItem> control_plan_items_b { get; set; }
    }
}