using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAdminService
    {
        Organization CreateNewOrganization();
        PersonRole CreateNewPersonRole();
        UserRole CreateNewUserRole();
        User CreateNewUser();
        OrganizationRole CreateNewOrganizationRole();
        MeasurableQuantity CreateNewMeasurableQuantity();
        Person CreateNewPerson();
        InstrumentType CreateNewInstrumentType();
    }
}
