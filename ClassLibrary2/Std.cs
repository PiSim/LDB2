using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Std
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Std()
        {
            this.Methods = new HashSet<Method>();
            this.Specifications = new HashSet<Specification>();
            this.StandardFiles = new HashSet<StandardFile>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int OrganizationID { get; set; }
        public string CurrentIssue { get; set; }
        public bool IsOutOfDate { get; set; }
        public Nullable<System.DateTime> LastIssueCheck { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Method> Methods { get; set; }
        public virtual Organization Organization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Specification> Specifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StandardFile> StandardFiles { get; set; }
    }
}