using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.ExternalReports = new HashSet<ExternalReport>();
            this.Materials = new HashSet<Material>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ProjectLeaderID { get; set; }
        public Nullable<int> OemID { get; set; }
        public double TotalExternalCost { get; set; }
        public double TotalReportDuration { get; set; }
        public Nullable<int> TBD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReport> ExternalReports { get; set; }
        public virtual Organization Oem { get; set; }
        public virtual Person Leader { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }
    }
}