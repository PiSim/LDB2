using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class OrganizationRoleMapping
    {
        public int ID { get; set; }

        public int OrganizationID { get; set; }
        public int OrganizationRoleID { get; set; }

        public Organization Organization { get; set; }
        public OrganizationRole OrganizationRole { get; set; }

        public bool IsSelected { get; set; }
    }
}
