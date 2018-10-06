using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class OrganizationRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrganizationRole()
        {
            this.OrganizationMappings = new HashSet<OrganizationRoleMapping>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrganizationRoleMapping> OrganizationMappings { get; set; }
    }
}