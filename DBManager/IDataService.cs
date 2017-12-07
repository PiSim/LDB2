using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Collections;

namespace DBManager
{
    public interface IDataService
    {
        IEnumerable<Batch> GetArchive();
        Batch GetBatch(int ID);
        Batch GetBatch(string batchNumber);
        IEnumerable<Batch> GetBatches();
        IEnumerable<Batch> GetBatches(int numberOfEntries);
        CalibrationReport GetCalibrationReport(int id);
        IEnumerable<CalibrationReport> GetCalibrationReports();
        Instrument GetInstrument(string code);
        IEnumerable<InstrumentType> GetInstrumentTypes();
        IEnumerable<InstrumentUtilizationArea> GetInstrumentUtilizationAreas();
        IEnumerable<MeasurableQuantity> GetMeasurableQuantities();
        IEnumerable<Organization> GetOrganizations();
        IEnumerable<Organization> GetOrganizations(string role);
        IEnumerable<OrganizationRole> GetOrganizationRoles();
        IEnumerable<Person> GetPeople(string role = null);
        IEnumerable<PersonRole> GetPersonRoles();
        IList<T> GetQueryResults<T>(IQuery<T> query);
        IEnumerable<Task> GetTasks(bool showComplete, bool showAssigned);
        IEnumerable<User> GetUsers();
        IEnumerable<Sample> GetSamples(int numberOfEntries = 0);
        IEnumerable<Property> GetProperties();
        IEnumerable<Instrument> GetInstruments();
        IEnumerable<MeasurementUnit> GetMeasurementUnits();
        IEnumerable<Project> GetProjects(bool includeCollections = false);
        TaskItem GetTaskItem(int iD);
        IEnumerable<Colour> GetColours();
        Material GetMaterial(MaterialType typeInstance, MaterialLine lineInstance, Aspect aspectInstance, Recipe recipeInstance);
        Aspect GetAspect(string aspectCode);
        MaterialLine GetMaterialLine(string lineCode);
        Recipe GetRecipe(string recipeCode);
        IEnumerable<TrialArea> GetTrialAreas();
        IEnumerable<ExternalConstruction> GetExternalConstructions();
        MaterialType GetMaterialType(string typeCode);
        IEnumerable<Aspect> GetAspects();
        Specification GetSpecification(string name);
        IEnumerable<Specification> GetSpecifications();
        Requirement GetRequirement(int ID);
        Std GetStandard(string name);
        SpecificationVersion GetSpecificationVersion(int ID);
        IEnumerable<Method> GetMethods();
        IEnumerable<Material> GetMaterialsWithoutConstruction();
    }
}
