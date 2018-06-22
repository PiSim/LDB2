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
        IEnumerable<ExternalReport> GetExternalReports();
        Instrument GetInstrument(string code);
        IEnumerable<Std> GetStandards();
        IEnumerable<InstrumentType> GetInstrumentTypes();
        IEnumerable<InstrumentUtilizationArea> GetInstrumentUtilizationAreas();
        IEnumerable<MeasurableQuantity> GetMeasurableQuantities();
        IEnumerable<Organization> GetOrganizations();
        IEnumerable<Organization> GetOrganizations(string role);
        IEnumerable<OrganizationRole> GetOrganizationRoles();
        IEnumerable<Person> GetPeople(string role = null);
        IEnumerable<PersonRole> GetPersonRoles();
        IList<T> GetQueryResults<T>(IQuery<T> query);
        IEnumerable<Report> GetReports();
        IEnumerable<Task> GetTasks(bool showComplete, bool showAssigned);
        IEnumerable<User> GetUsers();
        IEnumerable<Sample> GetSamples(int numberOfEntries = 0);
        IEnumerable<Property> GetProperties();
        IEnumerable<Instrument> GetInstruments();
        IEnumerable<MeasurementUnit> GetMeasurementUnits();
        IEnumerable<Project> GetProjects(bool includeCollections = false);
        TaskItem GetTaskItem(int iD);
        IEnumerable<Colour> GetColours();

        /// <summary>
        /// Queries for a material entity with the given type, line, aspect and color recipe
        /// If none is found returns null
        /// </summary>
        /// <param name="type">A string representing the material Type</param>
        /// <param name="line">A string representing the material Line</param>
        /// <param name="aspect">A string representing the material Aspect</param>
        /// <param name="recipe">A string representing the material Color Recipe</param>
        /// <returns>A Material instance with the given characteristics, or null if none is found</returns>
        Material GetMaterial(string type,
                            string line,
                            string aspect,
                            string recipe);

        /// <summary>
        /// Queries for a material entity with the given type, line, aspect and color recipe
        /// If none is found returns null
        /// </summary>
        /// <param name="type">A MaterialType instance</param>
        /// <param name="line">A MaterialLine instance</param>
        /// <param name="aspect">An Aspect </param>
        /// <param name="recipe">A string representing the material Color Recipe</param>
        /// <returns>A Material instance with the given characteristics, or null if none is found</returns>
        Material GetMaterial(MaterialType typeInstance, 
                            MaterialLine lineInstance, 
                            Aspect aspectInstance, 
                            Recipe recipeInstance);

        Aspect GetAspect(string aspectCode);
        MaterialLine GetMaterialLine(string lineCode);



        /// <summary>
        /// Returns a list of all the method entities in the database.
        /// </summary>
        /// <param name="includeObsolete">If false only the entries not marked
        /// as "obsolete" will be returned</param>
        /// <returns>An ICollection of Method Entities</returns>
        ICollection<Method> GetMethods(bool includeObsolete = false);

        /// <summary>
        /// Queries for a Recipe with the given ID
        /// </summary>
        /// <param name="recipeID">The Recipe ID to query for</param>
        /// <returns>The Recipe instance with the given ID, or null if none is found</returns>
        Recipe GetRecipe(int recipeID);

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
        

        IEnumerable<Material> GetMaterialsWithoutConstruction();
        Report GetReportByNumber(int number);
    }
}
