using DataAccess;
using LabDbContext;
using System.Collections.Generic;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command Object that updates the database values of a set of entities in a single transaction
    /// </summary>
    public class BulkUpdateEntitiesCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private IEnumerable<object> _entities;

        #endregion Fields

        #region Constructors

        public BulkUpdateEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;

            foreach (object entity in _entities)
            {
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}