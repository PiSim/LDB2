﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class SubMethod
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubMethod()
        {
            this.UM = "";
            this.SubRequirements = new HashSet<SubRequirement>();
            this.SubTaskItems = new HashSet<SubTaskItem>();
            this.SubTests = new HashSet<SubTest>();
            this.NewerVersions = new HashSet<SubMethod>();
        }

        public int ID { get; set; }
        public int MethodID { get; set; }
        public string Name { get; set; }
        public string UM { get; set; }
        public Nullable<int> OldVersionID { get; set; }
        public Nullable<int> Position { get; set; }

        public virtual Method Method { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubRequirement> SubRequirements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTaskItem> SubTaskItems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubTest> SubTests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SubMethod> NewerVersions { get; set; }
        public virtual SubMethod OldVersion { get; set; }
    }
}