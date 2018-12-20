using System.Collections.Generic;

namespace LInst
{
    public class User
    {
        #region Constructors

        public User()
        {
            FullName = "";
            UserName = "";
            HashedPassword = "";

            RoleMappings = new HashSet<UserRoleMapping>();
        }

        #endregion Constructors

        #region Properties

        public string FullName { get; set; }
        public string HashedPassword { get; set; }
        public int ID { get; set; }
        public Person Person { get; set; }
        public int PersonID { get; set; }
        public ICollection<UserRoleMapping> RoleMappings { get; set; }
        public string UserName { get; set; }

        #endregion Properties
    }
}