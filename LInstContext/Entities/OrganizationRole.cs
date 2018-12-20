using System.Collections.Generic;

namespace LInst
{
    public class OrganizationRole
    {
        #region Constructors

        public OrganizationRole()
        {
            OrganizationRoleMappings = new HashSet<OrganizationRoleMapping>();
        }

        #endregion Constructors

        #region Properties

        public string Description { get; set; }
        public int ID { get; set; }

        public string Name { get; set; }
        public ICollection<OrganizationRoleMapping> OrganizationRoleMappings { get; set; }

        #endregion Properties
    }
}