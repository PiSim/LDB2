using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class OrganizationRoleMapping
    {
        public int ID { get; set; }
        public int OrganizationID { get; set; }
        public int RoleID { get; set; }
        public bool IsSelected { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual OrganizationRole Role { get; set; }
    }
}