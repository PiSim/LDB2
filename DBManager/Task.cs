//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
        }
    
        public int ID { get; set; }
        public int RequesterID { get; private set; }
        public int projectID { get; set; }
        public int SpecificationVersionID { get; set; }
        public int batchID { get; set; }
        public string Notes { get; set; }
        public Nullable<int> reportID { get; set; }
        public Nullable<int> Progress { get; set; }
        public Nullable<int> PriorityModifier { get; set; }
        public string PipelineOrder { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public sbyte IsComplete { get; set; }
    
        public virtual Person Requester { get; set; }
        public virtual Project Project { get; set; }
        public virtual Report Report { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
