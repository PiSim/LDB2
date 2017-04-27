using DBManager;

namespace Infrastructure
{
    public interface IAdminServiceProvider
    {
        Person AddPerson();
        void AddPersonRole();
        void AddOrganizationRole(string name);
        void AddUserRole(string name);
    }
}
