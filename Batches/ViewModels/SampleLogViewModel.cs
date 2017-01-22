using DBManager;
using Infrastructure;
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
        private bool _materialChanged;
        private DBEntities _entities;
        private DelegateCommand _continue, _saveLogEntry;
        private IEventAggregator _eventAggregator;
        private List<Tuple<string, string>> _actions;
        private Material _material;
        private string _batchNumber;
        private Tuple<string, string> _selectedAction;

        public SampleLogViewModel(DBEntities entities, IEventAggregator eventAggregator) 
            : base()
        {
            _actions = new List<Tuple<string, string>>()
            {
                new Tuple<string, string> ("Arrivato in laboratorio", "A"),
                new Tuple<string, string> ("Buttato", "B"),
                new Tuple<string, string> ("Finito", "F"),
                new Tuple<string, string> ("Masterizzato", "M"),
                new Tuple<string, string> ("Spedito", "S")
            };

            _entities = entities;
            _eventAggregator = eventAggregator;

            _continue = new DelegateCommand(
                () =>
                {
                    if (_currentBatch == null)
                        CurrentBatch = _entities.GetBatchByNumber(BatchNumber);

                    else
                    {
                        _entities.CreateSampleForBatch(_currentBatch, _selectedAction.Item2);
                        _eventAggregator.GetEvent<CommitRequested>().Publish();
                        Clear();
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
            CurrentBatch = null;
        }

        public List<Tuple<string,string>> ActionList
        {
            get { return _actions; }
        }

        public Material BatchMaterial
        {
            get { return _material; }
            set
            {
                _material = value;
            }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                CurrentBatch = null;
            }
        }

        public Batch CurrentBatch
        {
            get { return _currentBatch; }
            set
            {
                _currentBatch = value;

                OnPropertyChanged("IsBatchLoaded");
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

        public bool IsBatchLoaded
        {
            get { return _currentBatch != null; }
        }

        public Tuple<string, string> SelectedAction
        {
            get { return _selectedAction; }
            set { _selectedAction = value; }
        }
    }
}
