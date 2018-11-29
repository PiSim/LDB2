using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    public class UpdateEntityCommand<T> : ICommand<T> where T : DbContext
    {
        private object _entity;

        public UpdateEntityCommand(object entity)
        {
            _entity = entity;
        }

        public void Execute(T context)
        {
            context.Update(_entity);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
