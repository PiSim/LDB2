using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Commands
{
    public class UpdateEntityCommand : ICommand<LabDbEntities>
    {
        private object _entity;

        public UpdateEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(LabDbEntities context)
        {
            context.Set(_entity.GetType()).Attach(_entity);
            context.Entry(_entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
