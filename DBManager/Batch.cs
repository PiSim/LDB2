//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabDbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class Batch
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Batch()
        {
            this.Notes = "";
            this.Masters = new HashSet<Master>();
            this.Reports = new HashSet<Report>();
            this.Samples = new HashSet<Sample>();
            this.Tasks = new HashSet<Task>();
            this.TestRecords = new HashSet<TestRecord>();
        }
    
        public int ID { get; set; }
        public string Number { get; set; }
        public int MaterialID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> TrialAreaID { get; set; }
        public bool FirstSampleArrived { get; set; }
        public Nullable<int> FirstSampleID { get; set; }
        public Nullable<int> BasicReportID { get; set; }
        public int ArchiveStock { get; set; }
        public bool DoNotTest { get; set; }
        public Nullable<int> LatestSampleID { get; set; }
        public Nullable<int> LongTermStock { get; set; }
        public string OrderFilePath { get; set; }
    
        public virtual Material Material { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Master> Masters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sample> Samples { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual TrialArea TrialArea { get; set; }
        public virtual Report BasicReport { get; set; }
        public virtual Sample FirstSample { get; set; }
        public virtual Sample LatestSample { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestRecord> TestRecords { get; set; }
    }
}
