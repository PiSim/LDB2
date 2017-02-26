using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class DBPrincipal
    {
        private DBIdentity _identity;

        public IIdentity Identity
        {
            get
            {
                return _identity ?? new DBIdentity();
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
    }
}
