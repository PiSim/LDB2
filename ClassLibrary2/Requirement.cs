﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Requirement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requirement()
        {
            this.Name = "";
            this.SubRequirements = new HashSet<SubRequirement>();
            this.Overrides = new HashSet<Requirement>();
            this.TaskItems = new HashSet<TaskItem>();
            this.tests = new HashSet<Test>();
            this.ControlPlanItems = new HashSet<ControlPlanItem>();
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public int Deprecated2 { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int SpecificationVersionID { get; set; }
        public bool IsOverride { get; set; }
        public Nullable<int> OverriddenID { get; set; }
        public bool SkipTest { get; set; }
        public Nullable<int> MethodVariantID { get; set; }

        public virtual Method Deprecated { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubRequirement> SubRequirements { get; set; }
        public virtual SpecificationVersion SpecificationVersions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requirement> Overrides { get; set; }
        public virtual Requirement Overridden { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TaskItem> TaskItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> tests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ControlPlanItem> ControlPlanItems { get; set; }
        public virtual MethodVariant MethodVariant { get; set; }
    }
}