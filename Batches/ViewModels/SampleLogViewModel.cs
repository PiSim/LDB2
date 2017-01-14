using DBManager;
using Prism.Commands;
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
        private DelegateCommand _saveLogEntry;
        private List<Tuple<string, string>> _actions;
        private string _batchNumber;

        public SampleLogViewModel(DBEntities entities) : base()
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

            _saveLogEntry = new DelegateCommand(
                () => 
                {
                    _currentBatch = entities.Batches.First(b => b.number == _batchNumber);
                } );
        }

        public List<Tuple<string,string>> ActionList
        {
            get { return _actions; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }

        public DelegateCommand SaveLogEntryCommand
        {
            get { return _saveLogEntry; }
        }
    }
}
