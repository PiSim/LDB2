using Infrastructure;
using LInst;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LabDB2
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Fields

        private IDesignTimeDbContextFactory<LInstContext> _contextFactory;

        #endregion Fields

        #region Constructors

        public AuthenticationService(IDesignTimeDbContextFactory<LInstContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        #endregion Constructors

        #region Methods

        public string CalculateHash(string clearText, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearText + salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashedBytes = hash.ComputeHash(saltedHashBytes);

            return Convert.ToBase64String(hashedBytes);
        }

        public LInst.User CreateNewUser(Person personInstance,
                                        string userName,
                                string password)
        {
            LInst.User output = new LInst.User();
            output.FullName = "";
            output.UserName = userName;
            output.HashedPassword = CalculateHash(password, userName);
            using (LInstContext context = _contextFactory.CreateDbContext(new string[] { }))
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

        #endregion Methods
    }
}