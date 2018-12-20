using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public partial class MethodVariant
    {
        #region Properties

        public string OemName => Method?.Standard?.Organization?.Name;

        public string PropertyName => Method?.Property?.Name;

        public string StandardName => Method?.Standard?.Name;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates a new Test entry from the MethodVariant.
        /// </summary>
        /// <returns>A newly generated, unattached Test entry</returns>
        public Test GenerateTest()
        {
            Test tempTest = new Test()
            {
                Duration = Method.Duration,
                Deprecated2 = Method.ID,
                MethodVariantID = ID,
                Notes = Name
            };

            foreach (SubMethod subMtd in Method.SubMethods)
            {
                SubTest tempSubTest = new SubTest()
                {
                    SubMethodID = subMtd.ID,
                    Name = subMtd.Name,
                    Position = subMtd.Position,
                    RequiredValue = "",
                    UM = subMtd.UM
                };
                tempTest.SubTests.Add(tempSubTest);
            }

            return tempTest;
        }

        /// <summary>
        /// Retrieves the Method for this Variant
        /// </summary>
        /// <param name="IncludeSubMethods">If true the query includes the SubMethods</param>
        /// <returns>A Method entity</returns>
        public Method GetMethod(bool IncludeSubMethods = false)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IQueryable<Method> query = entities.Methods;

                if (IncludeSubMethods)
                    query = query.Include(mtd => mtd.SubMethods);

                return query.FirstOrDefault(mtd => mtd.ID == MethodID);
            }
        }

        /// <summary>
        /// Deletes the entry from the database if no Test or requirement is associated with it, otherwise sets it as obsolete
        /// </summary>
        public void RemoveOrSetObsolete()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                MethodVariant attachedInstance = entities.MethodVariants.FirstOrDefault(mtdvar => mtdvar.ID == ID);
                if (attachedInstance == null)
                    return;

                if (attachedInstance.Requirements.Count == 0
                    && attachedInstance.Tests.Count == 0)
                    entities.Entry(attachedInstance)
                            .State = EntityState.Deleted;
                else
                {
                    attachedInstance.IsOld = true;
                    IsOld = true;
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}