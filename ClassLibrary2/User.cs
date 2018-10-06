using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.RoleMappings = new HashSet<UserRoleMapping>();
        }

        public int ID { get; set; }
        public string FullName { get; set; }
        public string HashedPassword { get; set; }
        public string UserName { get; set; }
        public Nullable<int> PersonID { get; set; }

        public virtual Person Person { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRoleMapping> RoleMappings { get; set; }
    }
}