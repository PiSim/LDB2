using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IDataService
    {
        T RunQuery<T>(IScalar<T> queryObject);
        IQueryable<T> RunQuery<T>(IQuery<T> queryObject);
        void Execute(ICommand commandObject);
    }
}
