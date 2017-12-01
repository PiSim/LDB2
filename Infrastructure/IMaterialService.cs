using DBManager;
using Infrastructure.Queries.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IMaterialService
    {
        Aspect CreateAspect();
        IEnumerable<IQueryPresenter<Batch>> GetBatchQueries();
    }
}
