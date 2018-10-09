using LabDbContext;

namespace Infrastructure
{
    public interface IAuthenticationService
    {
        #region Methods

        string CalculateHash(string clearText, string salt);
        LabDbContext.User CreateNewUser(Person personInstance, string userName, string password);


        #endregion Methods
    }
}