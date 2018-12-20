using DataAccess;
using LabDbContext;
using System.Collections.Generic;
using System.Data.Entity;

namespace Reports.Commands
{
    /// <summary>
    /// Command Object that updates the database values of the "Result" field from a set of SubTest entities in a single transaction
    /// </summary>
    public class BulkUpdateSubTestResultsCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private IEnumerable<SubTest> _entities;

        #endregion Fields

        #region Constructors

        public BulkUpdateSubTestResultsCommand(IEnumerable<SubTest> entities)
        {
            _entities = entities;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            DbSet<SubTest> subTests = context.SubTests;
            SubTest currentEntry;

            foreach (SubTest entity in _entities)
            {
                currentEntry = subTests.Find(entity.ID);
                currentEntry.Result = entity.Result;
                context.Entry(currentEntry).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}