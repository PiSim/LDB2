using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccessCore.Commands
{
    /// <summary>
    /// Command object that deletes multiple entries in a single transaction
    /// </summary>
    /// <typeparam name="T">The type of DbContext to target</typeparam>
    public class BulkDeleteEntitiesCommand<T> : ICommand<T> where T : DbContext
    {
        #region Constructors

        public BulkDeleteEntitiesCommand(IEnumerable<object> entryList)
        {
            EntryList = entryList;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<object> EntryList { get; set; }

        #endregion Properties

        #region Methods

        public void Execute(T context)
        {
            context.RemoveRange(EntryList);
            context.SaveChanges();
            context.Dispose();
        }

        #endregion Methods
    }
}