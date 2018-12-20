using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that deletes a given entry from the database
    /// </summary>
    public class DeleteEntityCommand<T> : ICommand<T> where T : DbContext
    {
        #region Fields

        private object _entity;

        #endregion Fields

        #region Constructors

        public DeleteEntityCommand()
        {
        }

        public DeleteEntityCommand(object entity)
        {
            _entity = entity;
        }

        #endregion Constructors

        #region Methods

        public void Execute(T context)
        {
            context.Remove(_entity);
            context.SaveChanges();
        }

        #endregion Methods
    }
}