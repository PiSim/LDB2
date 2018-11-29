
using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that deletes a given entry from the database
    /// </summary>
    public class DeleteEntityCommand<T> : ICommand<T> where T:DbContext
    {
        private object _entity;

        public DeleteEntityCommand()
        {
        }

        public DeleteEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(T context)
        {
            context.Remove(_entity);
            context.SaveChanges();
        }
    }
}
