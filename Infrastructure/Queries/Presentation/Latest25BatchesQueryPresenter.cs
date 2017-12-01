using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries.Presentation
{
    public class Latest25BatchesQueryPresenter : IQueryPresenter<Batch>
    {
        public string Name => "Ultimi 25 Batch";

        public string Description => "Restituisce gli ultimi 25 Batch in ordine di numero";

        public IQuery<Batch> Query => new LatestNBatchesQuery();
    }
}
