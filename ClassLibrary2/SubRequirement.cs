using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class SubRequirement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubRequirement()
        {
            this.RequiredValue = "";
            this.SubTests = new HashSet<SubTest>();
            this.SubTaskItems = new HashSet<SubTaskItem>();
        }

        public int ID { get; private set; }
        public string RequiredValue { get; set; }
        public int RequirementID { get; private set; }
        public int SubMethodID { get; set; }

        public virtual Requirement Requirement { get; set; }
        public virtual SubMethod SubMethod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTaskItem> SubTaskItems { get; set; }
    }
}