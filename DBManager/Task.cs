//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.TaskItems = new HashSet<TaskItem>();
            this.Reports = new HashSet<Report>();
        }
    
        public int ID { get; set; }
        public int RequesterID { get; private set; }
        public int SpecificationVersionID { get; set; }
        public int batchID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Progress { get; set; }
        public Nullable<int> PriorityModifier { get; set; }
        public Nullable<int> PipelineOrder { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsComplete { get; set; }
        public bool AllItemsAssigned { get; set; }
    
        public virtual Person Requester { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        public virtual Batch Batch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
