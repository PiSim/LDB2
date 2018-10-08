using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command Object that inserts a set of entities in a single transaction
    /// </summary>
    public class BulkInsertEntitiesCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<object> _entities;

        public BulkInsertEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (object entity in _entities)
                context.Set(entity.GetType()).Add(entity);

            context.SaveChanges();
        }
    }
}
