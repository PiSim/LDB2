using System;
using System.Collections.Generic;

namespace LabDbContext
{
    [Obsolete]
    public interface IDataService
    {
        #region Methods

        CalibrationReport GetCalibrationReport(int id);

        IEnumerable<MeasurableQuantity> GetMeasurableQuantities();

        IEnumerable<MeasurementUnit> GetMeasurementUnits();

        IEnumerable<OrganizationRole> GetOrganizationRoles();

        IEnumerable<PersonRole> GetPersonRoles();

        Requirement GetRequirement(int ID);

        IEnumerable<Task> GetTasks(bool showComplete, bool showAssigned);

        IEnumerable<User> GetUsers();

        #endregion Methods
    }
}