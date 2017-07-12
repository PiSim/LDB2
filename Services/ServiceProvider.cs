using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class ServiceProvider
    {


        internal static Person AddPerson()
        {
            StringInputDialog addPersonDialog = new StringInputDialog();
            addPersonDialog.Title = "Creazione nuova Persona";
            addPersonDialog.Message = "Nome:";

            if (addPersonDialog.ShowDialog() != true)
                return null;

            Person newPerson = new Person();
            newPerson.Name = addPersonDialog.InputString;

            foreach (PersonRole prr in PeopleService.GetPersonRoles())
            {
                PersonRoleMapping tempPRM = new PersonRoleMapping();
                tempPRM.Role = prr;
                tempPRM.IsSelected = false;
                newPerson.RoleMappings.Add(tempPRM);
            }

            newPerson.Create();

            return newPerson;
        }

        internal static void AddUserRole(string name)
        {
            UserRole newRole = new UserRole();
            newRole.Name = name;
            newRole.Description = "";

            newRole.Create();

            foreach (User usr in DataService.GetUsers())
            {
                UserRoleMapping newMap = new UserRoleMapping();
                newMap.IsSelected = false;
                newMap.RoleID = newRole.ID;
                newMap.UserID = usr.ID;

                newMap.Create();
            }
        }

        internal static Batch CreateBatch()
        {
            Views.BatchCreationDialog batchCreator = new Views.BatchCreationDialog();

            if (batchCreator.ShowDialog() == true)
            {
                return batchCreator.BatchInstance;
            }

            else
                return null;
        }

        public static Organization CreateNewOrganization()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuovo Ente";

            if (creationDialog.ShowDialog() == true)
            {
                Organization output = new Organization();
                output.Category = "";
                output.Name = creationDialog.InputString;
                foreach (OrganizationRole orr in DataService.GetOrganizationRoles())
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping();
                    tempORM.IsSelected = false;
                    tempORM.RoleID = orr.ID;

                    output.RoleMapping.Add(tempORM);
                }

                output.Create();

                return output;
            }
            else return null;
        }

        public static void CreateNewOrganizationRole()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuovo Ruolo Organizzazione";

            if (creationDialog.ShowDialog() == true)
            {
                OrganizationRole output = new OrganizationRole();
                output.Description = "";
                output.Name = creationDialog.InputString;
                output.Create();

                OrganizationService.CreateMappingsForNewRole(output);
            }

        }

        public static CalibrationReport RegisterNewCalibration(Instrument target)
        {
            Views.NewCalibrationDialog calibrationDialog = new Views.NewCalibrationDialog();
            calibrationDialog.InstrumentInstance = target;
            if (calibrationDialog.ShowDialog() == true)
            {
                CalibrationReport output = calibrationDialog.ReportInstance;

                DateTime tempNewDate = output.Date.AddMonths(target.ControlPeriod);
                if (tempNewDate > target.CalibrationDueDate)
                {
                    target.CalibrationDueDate = tempNewDate;
                    target.Update();
                }

                return output;
            }
            else return null;
        }
    }
}
