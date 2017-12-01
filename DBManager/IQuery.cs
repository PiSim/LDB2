
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public interface IQuery<T>
    {
        IQueryable<T> RunQuery(DBEntities entities);
    }
}
