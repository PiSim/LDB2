using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Report
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Report()
        {
            this.ReportFiles = new HashSet<ReportFile>();
            this.TBD = new HashSet<Test>();
            this.ParentTasks = new HashSet<Task>();
        }

        public int ID { get; set; }
        public int AuthorID { get; set; }
        public int BatchID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int Number { get; set; }
        public int SpecificationVersionID { get; set; }
        public string StartDate { get; set; }
        public bool IsComplete { get; set; }
        public double TotalDuration { get; set; }
        public int TestRecordID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ParentTaskID { get; set; }

        public virtual Batch Batch { get; set; }
        public virtual Person Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportFile> ReportFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> TBD { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> ParentTasks { get; set; }
        public virtual TestRecord TestRecord { get; set; }
    }
}