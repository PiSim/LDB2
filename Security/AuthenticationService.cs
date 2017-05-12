using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class AuthenticationService
    {
        private DBEntities _entities;

        public AuthenticationService(DBEntities entities)
        {
            _entities = entities;
        }

        public User AuthenticateUser(string userName, string password)
        {
            string hash = CalculateHash(password, userName);
            User authenticated = _entities.Users.FirstOrDefault(usr => usr.UserName == userName
                && usr.HashedPassword == hash);

            if (authenticated == null)
                throw new UnauthorizedAccessException();
            else
                return authenticated;
        }

        private string CalculateHash(string clearText, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearText + salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashedBytes = hash.ComputeHash(saltedHashBytes);

            return Convert.ToBase64String(hashedBytes);
        }

        public User CreateNewUser(Person personInstance,
                                string userName, 
                                string password)
        {
            User output = new User();
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
    }
}
