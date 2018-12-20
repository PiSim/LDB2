using DataAccess;
using LabDbContext;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command object that inserts a given entity in the database
    /// </summary>
    public class InsertEntityCommand : ICommand<LabDbEntities>
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

        public void Execute(LabDbEntities context)
        {
            context.Entry(_entity).State = System.Data.Entity.EntityState.Added;
            context.SaveChanges();
        }

        #endregion Methods
    }
}