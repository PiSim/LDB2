using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class SubTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubTest()
        {
            this.Result = "";
        }

        public int ID { get; set; }
        public string Result { get; set; }
        public int TestID { get; private set; }
        public string Name { get; set; }
        public string RequiredValue { get; set; }
        public string UM { get; set; }
        public Nullable<int> SubRequiremntID { get; set; }
        public Nullable<int> SubMethodID { get; set; }
        public Nullable<int> Position { get; set; }

        public virtual Test Test { get; set; }
        public virtual SubRequirement SubRequirement { get; set; }
        public virtual SubMethod SubMethod { get; set; }
    }
}