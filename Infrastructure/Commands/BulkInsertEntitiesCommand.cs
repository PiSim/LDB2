using DataAccess;
using LabDbContext;
using System.Collections.Generic;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command Object that inserts a set of entities in a single transaction
    /// </summary>
    public class BulkInsertEntitiesCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private IEnumerable<object> _entities;

        #endregion Fields

        #region Constructors

        public BulkInsertEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            foreach (object entity in _entities)
                context.Set(entity.GetType()).Add(entity);

            context.SaveChanges();
        }

        #endregion Methods
    }
}