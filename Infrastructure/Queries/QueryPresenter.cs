using DBManager;
using Infrastructure.Queries.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class QueryPresenter<T> : IQueryPresenter<T>
    {
        private string _description,
                        _name;
        private IQuery<T> _queryInstance;

        public QueryPresenter(IQuery<T> query,
                                string name,
                                string description = "")
        {
            _description = description;
            _name = name;
            _queryInstance = query;
        }

        public string Name => _name;

        public string Description => _description;

        public IQuery<T> Query => _queryInstance;
    }
}
