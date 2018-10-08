using System.Data.Entity;

namespace DataAccess
{
    public interface ICommand<T> where T : DbContext
    {
        #region Methods

        void Execute(T context);

        #endregion Methods
    }
}