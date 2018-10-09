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
    /// Command object that reloads all values for a given Entry
    /// </summary>
    public class ReloadEntityCommand : ICommand<LabDbEntities>
    {
        private object _entity;

        public ReloadEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(LabDbEntities context)
        {
            context.Set(_entity.GetType()).Attach(_entity);
            context.Entry(_entity).Reload();
            context.Dispose();
        }
    }
}
