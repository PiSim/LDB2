using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries.Presentation
{
    public class ArrivedUntestedBatchesQueryPresenter : IQueryPresenter<Batch>
    {
        private readonly string _name = "Batch arrivati ma non testati",
                                _description = "Restituisce una lista di Batch per cui è stato registrato l'arrivo di almeno un rotolo, " +
                                                "che non hanno Report e che non sono flaggati come da non testare";


        public IQuery<Batch> Query => new ArrivedUntestedBatchesQuery();

        public string Name => _name;

        public string Description => _description;
    }
}
