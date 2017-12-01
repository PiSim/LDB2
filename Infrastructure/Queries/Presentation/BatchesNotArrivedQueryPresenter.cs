using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries.Presentation
{
    public class BatchesNotArrivedQueryPresenter : IQueryPresenter<Batch>
    {
        public string Name => "Batch Non Arrivati";

        public string Description => "Restituisce i Batch di cui non sono stati ricevuti campioni";

        public IQuery<Batch> Query => new BatchesNotArrivedQuery();
    }
}
