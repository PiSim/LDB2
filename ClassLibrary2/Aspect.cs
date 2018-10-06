using System.Collections.Generic;

namespace LabDbContextCore
{
    public partial class Aspect
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Aspect()
        {
            this.Name = "";
            this.materials = new HashSet<Material>();
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> materials { get; set; }
    }
}