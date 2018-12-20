using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command Object that inserts a set of entities in a single transaction
    /// </summary>
    public class BulkInsertEntitiesCommand<T> : ICommand<T> where T : DbContext
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

        public void Execute(T context)
        {
            context.AddRange(_entities);
            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}