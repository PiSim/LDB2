using System.Collections.Generic;

namespace LInst
{
    public class Person
    {
        #region Constructors

        public Person()
        {
            RoleMappings = new HashSet<PersonRoleMapping>();
        }

        #endregion Constructors

        #region Properties

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<PersonRoleMapping> RoleMappings { get; set; }

        #endregion Properties
    }
}