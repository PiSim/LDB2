using Microsoft.EntityFrameworkCore;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that inserts a given entity in the database
    /// </summary>
    public class InsertEntityCommand<T> : ICommand<T> where T : DbContext
    {
        #region Fields

        private object _entity;

        #endregion Fields

        #region Constructors

        public InsertEntityCommand(object entity)
        {
            _entity = entity;
        }

        #endregion Constructors

        #region Methods

        public void Execute(T context)
        {
            context.Add(_entity);
            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}