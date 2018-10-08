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
    /// Command Object that updates the database values of a set of entities in a single transaction
    /// </summary>
    public class BulkUpdateEntitiesCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<object> _entities;

        public BulkUpdateEntitiesCommand(IEnumerable<object> entities)
        {
            _entities = entities;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (object entity in _entities)
            {
                context.Set(entity.GetType()).Attach(entity);
                context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            }

            context.SaveChanges();
        }
    }
}
