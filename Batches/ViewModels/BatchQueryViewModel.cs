using DBManager;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
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
        private Batch _selectedBatch;
        private DelegateCommand _openResult, _runQuery;
        private LabDBEntities _dbContext;
        private IRegionManager _regionManager;
        private ObservableCollection<Batch> _queryResults;

        public BatchQueryViewModel(LabDBEntities database, IRegionManager regions) : base()
        {
            _dbContext = database;
            _regionManager = regions;

            _openResult = new DelegateCommand(
                () => 
                {
                    NavigationParameters par = new NavigationParameters();
                    par.Add("batch", SelectedResult);
                    _regionManager.RequestNavigate(
                        Navigation.RegionNames.MainRegion,
                        new Uri(ViewNames.BatchInfoView, UriKind.Relative),
                        par);
                },
                () => SelectedResult != null);

            _runQuery = new DelegateCommand(
                () =>
                {
                    _queryResults = new ObservableCollection<Batch>(_dbContext.Batches);
                    OnPropertyChanged("QueryResults");
                });
        }

        public DelegateCommand OpenResultCommand
        {
            get { return _openResult; }
        }

        public ObservableCollection<Batch> QueryResults
        {
            get { return _queryResults; }
        }

        public DelegateCommand RunQueryCommand
        {
            get { return _runQuery; }
        }

        public Batch SelectedResult
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                OpenResultCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
