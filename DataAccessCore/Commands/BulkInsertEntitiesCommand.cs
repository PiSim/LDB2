using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command Object that inserts a set of entities in a single transaction
    /// </summary>
    public class BulkInsertEntitiesCommand<T> : ICommand<T> where T : DbContext
    {
        private IEnumerable<object> _entities;

        public BulkInsertEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        public void Execute(T context)
        {
            context.AddRange(_entities);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
