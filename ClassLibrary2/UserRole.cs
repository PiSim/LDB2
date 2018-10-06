using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class UserRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRole()
        {
            this.UserRoleMappings = new HashSet<UserRoleMapping>();
        }

        public int ID { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }
}