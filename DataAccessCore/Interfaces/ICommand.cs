using Microsoft.EntityFrameworkCore;

namespace DataAccessCore
{
    public interface ICommand<T> where T : DbContext
    {
        #region Methods

        void Execute(T context);

        #endregion Methods
    }
}