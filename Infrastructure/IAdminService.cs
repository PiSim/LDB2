using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IAdminService
    {
        void CreateNewOrganization();
        void CreateNewPersonRole();
        void CreateNewUserRole();
        void CreateNewUser();
        void CreateNewOrganizationRole();
        void CreateNewMeasurableQuantity();
        void CreateNewPerson();
        void CreateNewInstrumentType();
    }
}
