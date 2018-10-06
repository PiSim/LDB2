using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class MaterialLine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MaterialLine()
        {
            this.Materials = new HashSet<Material>();
        }

        public int ID { get; set; }
        public string Code { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }
    }
}