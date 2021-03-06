﻿using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Material
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            this.Batches = new HashSet<Batch>();
        }

        public int ID { get; set; }
        public int RecipeID { get; set; }
        public int LineID { get; set; }
        public int TypeID { get; set; }
        public int AspectID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> ExternalConstructionID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual Recipe Recipe { get; set; }
        public virtual Aspect Aspect { get; set; }
        public virtual ExternalConstruction ExternalConstruction { get; set; }
        public virtual MaterialLine MaterialLine { get; set; }
        public virtual MaterialType MaterialType { get; set; }
        public virtual Project Project { get; set; }
    }
}