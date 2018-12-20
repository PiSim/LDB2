using DataAccess;
using LabDbContext;

namespace Infrastructure.Commands
{
    /// <summary>
    /// Command object that reloads all values for a given Entry
    /// </summary>
    public class ReloadEntityCommand : ICommand<LabDbEntities>
    {
        #region Fields

        private object _entity;

        #endregion Fields

        #region Constructors

        public ReloadEntityCommand(object entity)
        {
            _entity = entity;
        }

        #endregion Constructors

        #region Methods

        public void Execute(LabDbEntities context)
        {
            context.Set(_entity.GetType()).Attach(_entity);
            context.Entry(_entity).Reload();
            context.Dispose();
        }

        #endregion Methods
    }
}