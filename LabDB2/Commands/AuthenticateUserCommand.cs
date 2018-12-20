using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LabDB2.Commands
{
    /// <summary>
    /// Command object for user authentication. Checks if a given username/hashedpassword combination exists in the database
    ///
    /// </summary>
    public class AuthenticateUserCommand : ICommand<LInstContext>
    {
        #region Fields

        private Action<LInst.User> _callBack;
        private LInst.User _toAuthenticate;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base Constructor for the command
        /// </summary>
        /// <param name="userToAuthenticate">A mocked user instance containing the username and password to authenticate</param>
        /// <param name="callBack">The method that will be called when the authentication is done, returns a connected authenticated user entry
        /// corresponding to the provided data, or null if none exists</param>
        public AuthenticateUserCommand(LInst.User userToAuthenticate,
                                        Action<LInst.User> callBack)
        {
            _toAuthenticate = userToAuthenticate;
            _callBack = callBack;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LInstContext context)
        {
            LInst.User authenticated = context.Users.Include(usr => usr.RoleMappings).ThenInclude(urm => urm.UserRole)
                                                            .AsNoTracking()
                                                            .FirstOrDefault(usr => usr.UserName == _toAuthenticate.UserName
                                                                                && usr.HashedPassword == _toAuthenticate.HashedPassword);
            _callBack(authenticated);
        }

        #endregion Methods
    }
}