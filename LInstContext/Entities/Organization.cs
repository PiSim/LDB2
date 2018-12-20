using System.Collections.Generic;

namespace LInst
{
    public class Organization
    {
        #region Constructors

        public Organization()
        {
            RoleMappings = new HashSet<OrganizationRoleMapping>();
        }

        #endregion Constructors

        #region Properties

        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<OrganizationRoleMapping> RoleMappings { get; set; }

        #endregion Properties
    }
}