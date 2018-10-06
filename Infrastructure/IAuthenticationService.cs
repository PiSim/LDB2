using LabDbContext;

namespace Infrastructure
{
    public interface IAuthenticationService
    {
        #region Methods

        LabDbContext.User AuthenticateUser(string userName, string password);

        LabDbContext.User CreateNewUser(Person personInstance, string userName, string password);

        #endregion Methods
    }
}