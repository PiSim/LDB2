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
    /// Command object that deletes a given entry from the database
    /// </summary>
    public class DeleteEntityCommand : ICommand<LabDbEntities>
    {
        private object _entity;

        public DeleteEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(LabDbEntities context)
        {
            context.Entry(_entity).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
