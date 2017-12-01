using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries.Presentation
{
    public interface IQueryPresenter<T>
    {
        string Name { get; }
        string Description { get; }
        IQuery<T> Query { get; }
    }
}
