using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Commands
{
    /// <summary>
    /// Command Object that updates the database values of the "Result" field from a set of SubTest entities in a single transaction
    /// </summary>
    public class BulkUpdateSubTestResultsCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<SubTest> _entities;

        public BulkUpdateSubTestResultsCommand(IEnumerable<SubTest> entities)
        {
            _entities = entities;
        }

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
    }
}
