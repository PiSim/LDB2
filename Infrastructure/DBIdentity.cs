using LInst;
using System.Security.Principal;

namespace Infrastructure
{
    public class DBIdentity : IIdentity
    {
        #region Constructors

        public DBIdentity()
        {
        }

        public DBIdentity(User authUser)
        {
            User = authUser;
        }

        #endregion Constructors

        #region Properties

        public string AuthenticationType => "Custom authentication";

        public bool IsAuthenticated => User != null;

        public string Name
        {
            get
            {
                if (User == null)
                    return null;

                return User.FullName ?? User.UserName;
            }
        }

        public User User { get; }

        #endregion Properties
    }
}