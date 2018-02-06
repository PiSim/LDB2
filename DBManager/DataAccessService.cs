using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DataAccessService : IDataService
    {

        public DataAccessService()
        {

        }


        public IEnumerable<Batch> GetArchive()
        {
            // Returns all the batches with a non-zero number of samples in stock

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.TrialArea)
                                        .Where(btc => btc.ArchiveStock > 0)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public Aspect GetAspect(string code)
        {
            // Returns an Aspect entity with the given code
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Aspects.FirstOrDefault(asp => asp.Code == code);
            }
        }

        public IEnumerable<Aspect> GetAspects()
        {
            // Returns all Aspect entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Aspects.Where(asp => true)
                                        .ToList();
            }
        }

        public IEnumerable<Batch> GetBatches()
        {
            // Returns all Batches

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .Where(btc => true)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public Batch GetBatch(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.ID == ID);
            }
        }

        public Batch GetBatch(string batchNumber)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.Number == batchNumber);
            }
        }

        public IEnumerable<Batch> GetBatches(int numberOfEntries)
        {
            // Returns the first numberOfEntries Batches by number descending

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .OrderByDescending(btc => btc.Number)
                                        .Take(numberOfEntries)
                                        .ToList();
            }
        }


        public CalibrationReport GetCalibrationReport(int ID)
        {
            // Returns a calibration report with the given ID, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports.FirstOrDefault(calrep => calrep.ID == ID);
            }
        }

        public IEnumerable<CalibrationReport> GetCalibrationReports()
        {
            // Returns all Calibrationreport entities, ordered by number descending

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Include(crep => crep.Instrument)
                                .Include(crep => crep.Laboratory)
                                .Include(crep => crep.CalibrationResult)
                                .Include(crep => crep.Tech)
                                .OrderByDescending(crep => crep.Year)
                                .ThenByDescending(crep => crep.Number)
                                .ToList();
            }
        }



        public IEnumerable<Colour> GetColours()
        {
            // Returns all Colour entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Colours.Where(clr => true)
                                        .OrderBy(clr => clr.Name)
                                        .ToList();
            }
        }


        public ControlPlan GetControlPlan(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.ControlPlans.First(entry => entry.ID == ID);
            }
        }

        public IEnumerable<ControlPlan> GetControlPlans()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.ControlPlans.Include(cpl => cpl.Specification)
                                            .OrderBy(cpl => cpl.Specification.Standard.Name)
                                            .ToList();
            }
        }

        public IEnumerable<ExternalConstruction> GetExternalConstructions()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalConstructions.Include(exc => exc.Oem)
                                                    .OrderBy(exc => exc.Oem.Name)
                                                    .ThenBy(exc => exc.Name)
                                                    .ToList();
            }
        }

        public Instrument GetInstrument(string code)
        {
            // Returns the instrument entry with the given code, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.FirstOrDefault(inst => inst.Code == code);
            }
        }

        public IEnumerable<Instrument> GetInstruments()
        {
            // Returns all Instrument entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.Include(inst => inst.InstrumentType)
                                            .Include(inst => inst.Manufacturer)
                                            .Include(inst => inst.InstrumentUtilizationArea)
                                            .OrderBy(inst => inst.Code)
                                            .ToList();
            }
        }


        public MaterialLine GetMaterialLine(string lineCode)
        {
            // Returns a MaterialLine entry with the given code, or null if none exists

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MaterialLines.FirstOrDefault(matl => matl.Code == lineCode);
            }
        }

        public IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            // Returns all InstrumentType entities

            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentTypes
                                .OrderBy(insty => insty.Name)
                                .ToList();
            }
        }


        public Material GetMaterial(int ID)
        {
            // Returns a Material entities with the given ID
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.ID == ID);
            }
        }

        public Material GetMaterial(string type,
                                    string line,
                                    string aspect,
                                    string recipe)
        {
            // Returns a Material entities with the type, line, aspect and recipe
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.Aspect.Code == aspect
                                                            && mat.MaterialLine.Code == line
                                                            && mat.Recipe.Code == recipe
                                                            && mat.MaterialType.Code == type);
            }
        }

        public  Material GetMaterial(MaterialType type,
                                    MaterialLine line,
                                    Aspect aspect,
                                    Recipe recipe)
        {
            // Returns a Material entities with the given construction and recipe
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.AspectID == aspect.ID
                                                            && mat.LineID == line.ID
                                                            && mat.RecipeID == recipe.ID
                                                            && mat.TypeID == type.ID);
            }
        }

        public IEnumerable<Material> GetMaterialsWithoutConstruction()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Materials.Where(mat => mat.ExternalConstruction == null)
                                            .Include(mat => mat.Aspect)
                                            .Include(mat => mat.MaterialLine)
                                            .Include(mat => mat.MaterialType)
                                            .Include(mat => mat.Recipe)
                                            .ToList();
            }
        }

        public IEnumerable<Material> GetMaterialsWithoutProject()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Materials.Where(mat => mat.Project == null)
                                            .Include(mat => mat.Aspect)
                                            .Include(mat => mat.ExternalConstruction)
                                            .Include(mat => mat.MaterialLine)
                                            .Include(mat => mat.MaterialType)
                                            .Include(mat => mat.Recipe)
                                            .ToList();
            }
        }

        public MaterialType GetMaterialType(string code)
        {
            // Returns a MaterialType entity with the given code
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MaterialTypes.FirstOrDefault(mty => mty.Code == code);
            }
        }

        public IEnumerable<MeasurableQuantity> GetMeasurableQuantities()
        {
            // Returns all Measurable Quantities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.ToList();
            }
        }
        public IEnumerable<MeasurementUnit> GetMeasurementUnits()
        {
            // Returns all measurement units

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.ToList();
            }
        }

        public Method GetMethod(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Methods.First(entry => entry.ID == ID);
            }
        }

        public IEnumerable<Method> GetMethods()
        {
            // Returns all Method entities.

            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Standard.Organization)
                                        .Include(mtd => mtd.Property)
                                        .Where(mtd => true)
                                        .OrderBy(mtd => mtd.Standard.Name)
                                        .ToList();
            }
        }



        public IEnumerable<Organization> GetOrganizations()
        {
            // Returns all Organization entities, filtering by role if one is provided

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Organizations.Include(org => org.RoleMapping
                                                .Select(orm => orm.Role))
                                                .OrderBy(org => org.Name)
                                                .ToList();
            }
        }

        public IEnumerable<Organization> GetOrganizations(string roleName)
        {
            // Returns all Organization entities, filtering by role if one is provided

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Organizations.Include(org => org.RoleMapping
                                                .Select(orm => orm.Role))
                                                .Where(org => org.RoleMapping
                                                .FirstOrDefault(orm => orm.Role.Name == roleName)
                                                .IsSelected)
                                                .OrderBy(org => org.Name)
                                                .ToList();
            }
        }

        /// <summary>
        /// Returns all OrganizationRoles
        /// </summary>
        /// <returns>An IEnumerable containing all the OrganizationRole entities</returns>
        public IEnumerable<OrganizationRole> GetOrganizationRoles()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoles
                                .ToList();
            }
        }

        public IEnumerable<Person> GetPeople(string roleName = null)
        {
            // Returns all People entities, a rolename can be provided to filter by

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.People.Where(per => roleName == null || per.RoleMappings
                                        .FirstOrDefault(prm => prm.Role.Name == roleName)
                                        .IsSelected)
                                        .OrderBy(per => per.Name)
                                        .ToList();
            }
        }

        /// <summary>
        /// Returns all PersonRole Entities
        /// </summary>
        /// <returns>An IEnumerable containing all PersonRole entities</returns>
        public IEnumerable<PersonRole> GetPersonRoles()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PersonRoles.ToList();
            }
        }



        public IEnumerable<Project> GetProjects(bool includeCollections = false)
        {
            // Returns all Project entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                if (includeCollections)
                    return entities.Projects.Include(prj => prj.Leader)
                                            .Include(prj => prj.Oem)
                                            .Include(prj => prj.ExternalReports)
                                            .Include(prj => prj.Materials
                                            .Select(mat => mat.Batches
                                            .Select(btc => btc.Reports)))
                                            .OrderByDescending(prj => prj.Name)
                                            .ToList();


                else
                    return entities.Projects.Include(prj => prj.Leader)
                                            .Include(prj => prj.Oem)
                                            .OrderByDescending(prj => prj.Name)
                                            .ToList();
            }
        }

        public IEnumerable<Property> GetProperties()
        {
            // Returns all Property entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Properties.Where(prp => true)
                                            .ToList();
            }
        }

        public IList<T> GetQueryResults<T>(IQuery<T> query)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return query.RunQuery(entities)
                            .ToList();
            }
        }

        /// <summary>
        /// Returns an ordered list of inserted samples. If an int is provided the last *int* samples 
        /// are returned. 
        /// </summary>
        /// <param name="numberOfEntries">The number of entries that will be returned. If 0 or none is 
        /// provided, all entries will be returned</param>
        /// <returns>An IEnumerable with the found samples</returns>
        public IEnumerable<Sample> GetSamples(int numberOfEntries = 0)
        {
            // Returns a given number of the most recently inserted samples

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<Sample> query = entities.Samples.Include(smp => smp.Batch)
                                                            .Include(smp => smp.LogAuthor)
                                                            .OrderByDescending(smp => smp.ID);

                if (numberOfEntries == 0)
                    return query.ToList();

                else
                    return query.Take(numberOfEntries)
                                .ToList();
            }
        }



        public Recipe GetRecipe(string code)
        {
            // Returns the recipe with the given code
            // if none is found, null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Recipes.FirstOrDefault(rec => rec.Code == code);
            }
        }

        public Requirement GetRequirement(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.First(req => req.ID == ID);
            }
        }

        public Specification GetSpecification(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Specifications.First(entry => entry.ID == ID);
            }
        }

        public Specification GetSpecification(string name)
        {
            // returns a specification with the given Standard name, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Specifications.FirstOrDefault(spec => spec.Standard.Name == name);
            }
        }

        public IEnumerable<Specification> GetSpecifications()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Specifications.Include(spec => spec.Standard.Organization)
                                                .OrderBy(spec => spec.Standard.Organization.Name)
                                                .ThenBy(spec => spec.Standard.Name)
                                                .ToList();
            }
        }

        public SpecificationVersion GetSpecificationVersion(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SpecificationVersions.FirstOrDefault(specv => specv.ID == ID);
            }

        }

        public Std GetStandard(string name)
        {
            // returns Standard entity with the provided name or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Stds.Include(std => std.Organization)
                                    .FirstOrDefault(std => std.Name == name);
            }
        }

        /// <summary>
        /// Returns all the Std entities existing in the DB
        /// </summary>
        /// <returns>An IEnumerable of Std entities</returns>
        public IEnumerable<Std> GetStandards()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Stds
                                .ToList();
            }
        }

        /// <summary>
        /// Returns the TaskItem with the given ID
        /// </summary>
        /// <param name="ID">The ID to look up</param>
        /// <returns>The task item with the given ID, or null if none is found</returns>
        public TaskItem GetTaskItem(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems.FirstOrDefault(tski => tski.ID == ID);
            }
        }

        /// <summary>
        /// Returns the task entry with the given ID
        /// </summary>
        /// <param name="ID">the ID to look up</param>
        /// <returns>The task entry with the given ID, or null if none is found</returns>
        public Task GetTask(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.FirstOrDefault(tsk => tsk.ID == ID);
            }
        }

       /// <summary>
       /// Returns all Task entities in the DB
       /// </summary>
       /// <param name="includeComplete">If true complete tasks will be included</param>
       /// <param name="includeAssigned">If true assigned tasks will be included</param>
       /// <returns>IEnumerable containing all the found entries</returns>
        public IEnumerable<Task> GetTasks(bool includeComplete = true,
                                        bool includeAssigned = true)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<Task> queryBase = entities.Tasks.Include(tsk => tsk.Batch.Material.Aspect)
                                                            .Include(tsk => tsk.Batch.Material.Project)
                                                            .Include(tsk => tsk.Batch.Material.MaterialLine)
                                                            .Include(tsk => tsk.Batch.Material.MaterialType)
                                                            .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                            .Include(tsk => tsk.Requester)
                                                            .Include(tsk => tsk.SpecificationVersion.Specification.Standard);

                if (includeComplete)
                    return queryBase.ToList();

                if (includeAssigned)
                    return queryBase.Where(tsk => tsk.Report == null || !tsk.Report.IsComplete)
                                    .ToList();

                else
                    return queryBase.Where(tsk => tsk.Report == null)
                                    .ToList();
            }

        }

        public IEnumerable<TrialArea> GetTrialAreas()
        {
            // Returns all TrialArea entries

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TrialAreas.Where(tra => true)
                                            .ToList();
            }
        }

        public IEnumerable<User> GetUsers()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Users.ToList();
            }
        }

        public IEnumerable<InstrumentUtilizationArea> GetInstrumentUtilizationAreas()
        {

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentUtilizationAreas
                                .OrderBy(iua => iua.Name)
                                .ToList();
            }
        }
    }
}
