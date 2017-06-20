using DBManager;
using DBManager.EntityExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class ServiceProvider
    {
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
