
using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that inserts a given entity in the database
    /// </summary>
    public class InsertEntityCommand<T> : ICommand<T> where T : DbContext
    {
        private object _entity;

        public InsertEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(T context)
        {
            context.Add(_entity);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
