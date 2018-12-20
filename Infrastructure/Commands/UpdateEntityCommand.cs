using DataAccess;
using LabDbContext;

namespace Infrastructure.Commands
{
    public class UpdateEntityCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private object _entity;

        #endregion Fields

        #region Constructors

        public UpdateEntityCommand(object entity)
        {
            _entity = entity;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Set(_entity.GetType()).Attach(_entity);
            context.Entry(_entity).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}