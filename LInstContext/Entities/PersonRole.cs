using System.Collections.Generic;

namespace LInst
{
    public class PersonRole
    {
        #region Properties

        public string Description { get; set; }
        public int ID { get; set; }

        public string Name { get; set; }
        public ICollection<PersonRoleMapping> RoleMappings { get; set; }

        #endregion Properties
    }
}