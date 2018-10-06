using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class TaskItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskItem()
        {
            this.SubTaskItems = new HashSet<SubTaskItem>();
        }

        public int ID { get; set; }
        public int TaskID { get; set; }
        public Nullable<int> RequirementID { get; set; }
        public Nullable<int> TestID { get; set; }
        public int SpecificationVersionID { get; set; }
        public int MethodID { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }
        public double WorkHours { get; set; }

        public virtual Requirement Requirement { get; set; }
        public virtual Task Task { get; set; }
        public virtual Test Test { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTaskItem> SubTaskItems { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
        public virtual Method Method { get; set; }
    }
}