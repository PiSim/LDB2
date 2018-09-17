using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class DataService<T> where T : DBContext , IDataService
    {
        private IDBContextFactory<T> _contextFactory ;


        public DataService(IDBContextFactory<T> contextFactory )
        {
            _contextFactory = contextFactory;
        }

        public void Execute(ICommand commandObject)
        {
            commandObject.Execute(_contextFactory.Create());
        }

        public T2 RunQuery<T2>(IScalar<T2> queryObject)
        {
            return queryObject.Execute(_contextFactory.Create());
        }

        public IQueryable<T2> RunQuery<T2>(IQuery<T2> queryObject)
        {
            return queryObject.Execute(_contextFactory.Create());
        }
    }
}
