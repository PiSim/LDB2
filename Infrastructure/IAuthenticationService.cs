using LInst;

namespace Infrastructure
{
    public interface IAuthenticationService
    {
        #region Methods

        string CalculateHash(string clearText, string salt);

        User CreateNewUser(Person personInstance, string userName, string password);

        #endregion Methods
    }
}