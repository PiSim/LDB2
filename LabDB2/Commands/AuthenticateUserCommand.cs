using DataAccess;
using LabDbContext;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabDB2.Commands
{
    /// <summary>
    /// Command object for user authentication. Checks if a given username/hashedpassword combination exists in the database
    /// 
    /// </summary>
    public class AuthenticateUserCommand : ICommand<LabDbEntities>
    {
        private LabDbContext.User _toAuthenticate;
        private Action<LabDbContext.User> _callBack;

        /// <summary>
        /// Base Constructor for the command
        /// </summary>
        /// <param name="userToAuthenticate">A mocked user instance containing the username and password to authenticate</param>
        /// <param name="callBack">The method that will be called when the authentication is done, returns a connected authenticated user entry
        /// corresponding to the provided data, or null if none exists</param>
        public AuthenticateUserCommand(LabDbContext.User userToAuthenticate,
                                        Action<LabDbContext.User> callBack)
        {
            _toAuthenticate = userToAuthenticate;
            _callBack = callBack;
        }

        public void Execute(LabDbEntities context)
        {
            LabDbContext.User authenticated = context.Users.Include(usr => usr.RoleMappings
                                                            .Select(urm => urm.UserRole))
                                                            .AsNoTracking()
                                                            .FirstOrDefault(usr => usr.UserName == _toAuthenticate.UserName
                                                                                && usr.HashedPassword == _toAuthenticate.HashedPassword);
            _callBack(authenticated);
        }
    }
}
