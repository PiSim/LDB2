using System.Collections.Generic;

namespace LInst
{
    public class UserRole
    {
        #region Constructors

        public UserRole()
        {
            Name = "";
            Description = "";

            UserMappings = new HashSet<UserRoleMapping>();
        }

        #endregion Constructors

        #region Properties

        public string Description { get; set; }
        public int ID { get; set; }

        public string Name { get; set; }
        public ICollection<UserRoleMapping> UserMappings { get; set; }

        #endregion Properties
    }
}