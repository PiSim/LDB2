using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public static class RequirementExtension
    {
        #region Methods

        public static void Create(this Requirement entry)
        {
            // Insert new Requirement entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Requirements.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Requirement entry)
        {
            // Deletes Requirement entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .Requirements
                        .First(req => req.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static void Load(this Requirement entry)
        {
            // Loads DB values a given Requirement entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Requirements.Attach(entry);

                Requirement tempEntry = entities.Requirements.Include(req => req.MethodVariant.Method.Property)
                                                                .Include(req => req.MethodVariant.Method.Standard.Organization)
                                                                .Include(req => req.Overridden)
                                                                .Include(req => req.SubRequirements)
                                                                .First(req => req.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        #endregion Methods
    }

    public partial class Requirement
    {
        #region Properties

        public string VariantName => MethodVariant?.Name;

        #endregion Properties
    }
}