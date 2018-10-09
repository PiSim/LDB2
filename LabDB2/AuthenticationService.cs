using Infrastructure;
using LabDbContext;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LabDB2
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _contextFactory;

        #endregion Fields

        #region Constructors

        public AuthenticationService(IDbContextFactory<LabDbEntities> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods
        
        public LabDbContext.User CreateNewUser(Person personInstance,
                                string userName,
                                string password)
        {
            LabDbContext.User output = new LabDbContext.User();
            output.FullName = "";
            output.UserName = userName;
            output.HashedPassword = CalculateHash(password, userName);
            using (LabDbEntities context = _contextFactory.Create())
            {
                output.Person = context.People.First(per => per.ID == personInstance.ID);
                foreach (UserRole role in context.UserRoles)
                {
                    UserRoleMapping tempMapping = new UserRoleMapping();
                    tempMapping.UserRole = role;
                    tempMapping.IsSelected = false;

                    output.RoleMappings.Add(tempMapping);
                }
                context.Users.Add(output);
                context.SaveChanges();
            }

            return output;
            

            
        }

        public string CalculateHash(string clearText, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearText + salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashedBytes = hash.ComputeHash(saltedHashBytes);

            return Convert.ToBase64String(hashedBytes);
        }

        #endregion Methods
    }
}