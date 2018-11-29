using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command Object that updates the database values of a set of entities in a single transaction
    /// </summary>
    public class BulkUpdateEntitiesCommand<T> : ICommand<T> where T:DbContext
    {
        private IEnumerable<object> _entities;

        public BulkUpdateEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        public void Execute(T context)
        {
            context.UpdateRange(_entities);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
