using LInst;
using System.Linq;
using System.Security.Principal;

namespace Infrastructure
{
    public class DBPrincipal : IPrincipal
    {
        #region Fields

        private DBIdentity _identity;

        #endregion Fields

        #region Properties

        public Person CurrentPerson => _identity.User.Person;

        public User CurrentUser => _identity.User;

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

        #endregion Properties

        #region Methods

        public bool IsInRole(string role)
        {
            UserRoleMapping tempURM = _identity.User.RoleMappings.First(urm => urm.UserRole.Name == role);

            return tempURM.IsSelected;
        }

        #endregion Methods
    }
}