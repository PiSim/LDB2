using Controls.Views;
using DBManager;
using Infrastructure.Events;
using Materials;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    class SampleLogViewModel : BindableBase
    {
        private DelegateCommand _continue;
        private EventAggregator _eventAggregator;
        private List<Tuple<string, string>> _actions;
        private string _batchNumber;
        private Tuple<string, string> _selectedAction;

        public SampleLogViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _actions = new List<Tuple<string, string>>()
            {
                new Tuple<string, string> ("Arrivato in laboratorio", "A"),
                new Tuple<string, string> ("Buttato", "B"),
                new Tuple<string, string> ("Finito", "F"),
                new Tuple<string, string> ("Masterizzato", "M"),
                new Tuple<string, string> ("Spedito", "S")
            };

            _continue = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<SampleCreationRequested>()
                                    .Publish(new Tuple<string, string>(_batchNumber, _selectedAction.Item2));
                    Clear();
                });
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
                RaisePropertyChanged("BatchNumber");
            }
        }

        public DelegateCommand ContinueCommand
        {
            get { return _continue; }
        }
        
        public Tuple<string, string> SelectedAction
        {
            get { return _selectedAction; }
            set { _selectedAction = value; }
        }
    }
}
