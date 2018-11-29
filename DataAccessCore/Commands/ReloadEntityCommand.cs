
using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that reloads all values for a given Entry
    /// </summary>
    public class ReloadEntityCommand<T> : ICommand<T> where T:DbContext
    {
        private object _entity;

        public ReloadEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(T context)
        {
            context.Attach(_entity);
            context.Entry(_entity).Reload();
            context.Dispose();
        }
    }
}
