using System;
using System.Collections.Generic;
using System.Text;

namespace LInst
{
    public class Organization
    {
        public Organization()
        {
            RoleMappings = new HashSet<OrganizationRoleMapping>();
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<OrganizationRoleMapping> RoleMappings { get; set; }
    }
}
