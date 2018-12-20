using DataAccess;
using LabDbContext;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command object that deletes a given entry from the database
    /// </summary>
    public class DeleteEntityCommand : ICommand<LabDbEntities>
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

        public void Execute(LabDbEntities context)
        {
            context.Entry(_entity).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }

        #endregion Methods
    }
}