using LabDbContext;

namespace Infrastructure
{
    public interface IAdminService
    {
        #region Methods

        InstrumentType CreateNewInstrumentType();

        MeasurableQuantity CreateNewMeasurableQuantity();

        Organization CreateNewOrganization();

        OrganizationRole CreateNewOrganizationRole();

        Person CreateNewPerson();

        PersonRole CreateNewPersonRole();

        Property CreateNewProperty();

        User CreateNewUser();

        UserRole CreateNewUserRole();

        #endregion Methods
    }
}