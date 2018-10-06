using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Recipe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipe()
        {
            this.Materials = new HashSet<Material>();
        }

        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<int> ColourID { get; set; }
        public Nullable<int> masterID { get; set; }

        public virtual Colour Colour { get; set; }
        public virtual Master Master { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }
    }
}