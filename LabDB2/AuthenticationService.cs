using Infrastructure;
using LabDbContext;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LabDB2
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private LabDbEntities _entities;

        #endregion Fields

        #region Constructors

        public AuthenticationService(LabDbEntities entities)
        {
            _entities = entities;
        }

        #endregion Constructors

        #region Methods

        public LabDbContext.User AuthenticateUser(string userName, string password)
        {
            string hash = CalculateHash(password, userName);
            LabDbContext.User authenticated = _entities.Users.FirstOrDefault(usr => usr.UserName == userName
                && usr.HashedPassword == hash);

            if (authenticated == null)
                throw new UnauthorizedAccessException();
            else
                return authenticated;
        }

        public LabDbContext.User CreateNewUser(Person personInstance,
                                string userName,
                                string password)
        {
            LabDbContext.User output = new LabDbContext.User();
            output.FullName = "";
            output.UserName = userName;
            output.HashedPassword = CalculateHash(password, userName);
            output.Person = _entities.People.First(per => per.ID == personInstance.ID);

            foreach (UserRole role in _entities.UserRoles)
            {
                UserRoleMapping tempMapping = new UserRoleMapping();
                tempMapping.UserRole = role;
                tempMapping.IsSelected = false;

                output.RoleMappings.Add(tempMapping);
            }
            _entities.Users.Add(output);
            _entities.SaveChanges();

            return output;
        }

        private string CalculateHash(string clearText, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearText + salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashedBytes = hash.ComputeHash(saltedHashBytes);

            return Convert.ToBase64String(hashedBytes);
        }

        #endregion Methods
    }
}