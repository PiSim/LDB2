using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Method
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Method()
        {
            this.TBD = " ";
            this.Deprecated2 = new HashSet<Requirement>();
            this.Tests = new HashSet<Test>();
            this.SubMethods = new HashSet<SubMethod>();
            this.AssociatedInstruments = new HashSet<Instrument>();
            this.Deprecated = new HashSet<ExternalReport>();
            this.TaskItems = new HashSet<TaskItem>();
            this.NewerVersions = new HashSet<Method>();
            this.MethodVariants = new HashSet<MethodVariant>();
        }

        public int ID { get; private set; }
        public int StandardID { get; set; }
        public int PropertyID { get; set; }
        public string Description { get; set; }
        public string TBD { get; set; }
        public double Duration { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<int> OldVersionID { get; set; }
        public bool IsOld { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requirement> Deprecated2 { get; set; }
        public virtual Property Property { get; set; }
        public virtual Std Standard { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubMethod> SubMethods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Instrument> AssociatedInstruments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalReport> Deprecated { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Method> NewerVersions { get; set; }
        public virtual Method OldVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MethodVariant> MethodVariants { get; set; }
    }
}