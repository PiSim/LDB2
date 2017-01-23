using Controls.Views;
using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batches.ViewModels
{
    class SampleLogViewModel : BindableBase
    {
        private Batch _currentBatch;
        private DBEntities _entities;
        private DelegateCommand _continue, _saveLogEntry;
        private IEventAggregator _eventAggregator;
        private IUnityContainer _container;
        private List<Tuple<string, string>> _actions;
        private string _batchNumber;
        private Tuple<string, string> _selectedAction;

        public SampleLogViewModel(DBEntities entities, IEventAggregator eventAggregator,
                IUnityContainer container) : base()
        {
            _actions = new List<Tuple<string, string>>()
            {
                new Tuple<string, string> ("Arrivato in laboratorio", "A"),
                new Tuple<string, string> ("Buttato", "B"),
                new Tuple<string, string> ("Finito", "F"),
                new Tuple<string, string> ("Masterizzato", "M"),
                new Tuple<string, string> ("Spedito", "S")
            };

            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;

            _continue = new DelegateCommand(
                () =>
                {
                    _currentBatch = _entities.GetBatchByNumber(_batchNumber);
                    if (_currentBatch.Material == null)
                    {
                        MaterialCreationDialog matDialog = _container.Resolve<MaterialCreationDialog>();
                        if (matDialog.ShowDialog() == true)
                            _currentBatch.Material = matDialog.ValidatedMaterial;
                    }
                });

            _saveLogEntry = new DelegateCommand(
                () => 
                {
                    _currentBatch = entities.Batches.First(b => b.Number == _batchNumber);
                } );
        }

        public void Clear()
        {
            BatchNumber = null;
        }

        public List<Tuple<string,string>> ActionList
        {
            get { return _actions; }
        }
        
        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                OnPropertyChanged("BatchNumber");
            }
        }

        public DelegateCommand ContinueCommand
        {
            get { return _continue; }
        }

        public DelegateCommand SaveLogEntryCommand
        {
            get { return _saveLogEntry; }
        }
        
        public Tuple<string, string> SelectedAction
        {
            get { return _selectedAction; }
            set { _selectedAction = value; }
        }
    }
}
