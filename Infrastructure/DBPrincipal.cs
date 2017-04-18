using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class DBPrincipal : IPrincipal
    {
        private DBIdentity _identity;

        public IIdentity Identity
        {
            get
            {
                if (_identity == null)
                    _identity = new DBIdentity();
                    
                return _identity;
            }
            set
            {
                _identity = value as DBIdentity;
            }
        }

        public bool IsInRole(string role)
        {
            return _identity.User.RoleMappings.Any(urm => urm.UserRole.Name == role);
        }

        public Person CurrentPerson
        {
            get { return _identity.User.Person; }
        }
        
        public User CurrentUser
        {
            get { return _identity.User; }
        }
    }
}