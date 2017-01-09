using DBManager;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Batches.ViewModels
{
    class BatchQueryViewModel : BindableBase
    {
        private DelegateCommand _runQuery;
        private DbContext _dbContext;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<Batch> _queryResults;

        public BatchQueryViewModel(DbContext database, IEventAggregator eventAggregator) : base()
        {
            _dbContext = database;
            _eventAggregator = eventAggregator;
            _runQuery = new DelegateCommand(
                () =>
                {

                });
        }

        public ObservableCollection<Batch> QueryResults
        {
            get { return _queryResults; }
        }

        public DelegateCommand RunQueryCommand
        {
            get { return _runQuery; }
        }
    }
}
