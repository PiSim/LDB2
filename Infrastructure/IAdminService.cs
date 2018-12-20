using LInst;

namespace Infrastructure
{
    public interface IAdminService
    {
        #region Methods

        InstrumentType CreateNewInstrumentType();

        Organization CreateNewOrganization();

        OrganizationRole CreateNewOrganizationRole();

        Person CreateNewPerson();

        PersonRole CreateNewPersonRole();

        User CreateNewUser();

        UserRole CreateNewUserRole();

        #endregion Methods
    }
}