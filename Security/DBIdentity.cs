using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    class DBIdentity : IIdentity
    {
        private User _user;

        public DBIdentity()
        {

        }

        public DBIdentity(User authUser)
        {
            _user = authUser;
        }

        public string Name
        {
            get
            {
                if (_user == null)
                    return null;

                return _user.FullName ?? _user.UserName;
            }
        }

        public string AuthenticationType
        {
            get { return "Custom authentication"; }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _user != null;
            }
        }

        public User User
        {
            get { return _user; }
        }
    }
}