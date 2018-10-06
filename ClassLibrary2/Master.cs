using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Master()
        {
            this.Recipes = new HashSet<Recipe>();
        }

        public int ID { get; set; }
        public int Number { get; set; }
        public string Date { get; set; }
        public Nullable<int> BatchID { get; private set; }

        public virtual Batch Batch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}