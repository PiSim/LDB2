using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class MethodVariant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MethodVariant()
        {
            this.Name = "Variante";
            this.Description = " ";
            this.Requirements = new HashSet<Requirement>();
            this.Tests = new HashSet<Test>();
            this.ExternalReports = new HashSet<ExternalReport>();
            this.NewerVersion = new HashSet<MethodVariant>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MethodID { get; set; }
        public Nullable<int> PreviousVersionID { get; set; }
        public bool IsOld { get; set; }

        public virtual Method Method { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requirement> Requirements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReport> ExternalReports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodVariant> NewerVersion { get; set; }
        public virtual MethodVariant PreviousVersion { get; set; }
    }
}