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
    
    public partial class TaskItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaskItem()
        {
            this.Tests = new HashSet<Test>();
            this.SubTaskItems = new HashSet<SubTaskItem>();
        }
    
        public int ID { get; set; }
        public int TaskID { get; private set; }
        public int RequirementID { get; set; }
        public bool IsAssignedToReport { get; set; }
        public Nullable<int> TestID { get; set; }
        public int SpecificationVersionID { get; set; }
        public Nullable<int> MethodID { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }
    
        public virtual Requirement Requirement { get; set; }
        public virtual Task Task { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual Test Test { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTaskItem> SubTaskItems { get; set; }
        public virtual SpecificationVersion SpecificationVersion { get; set; }
    }
}
