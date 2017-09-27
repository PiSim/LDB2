using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{
    public class SampleLogDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private Batch _batchInstance;
        private DBPrincipal _principal;
        private DelegateCommand _confirm, 
                                _deleteLast;
        private DelegateCommand<Window> _end;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private readonly Dictionary<string, int> _stockModifiers = new Dictionary<string, int>()
                                                                {
                                                                    {"A", 1 },
                                                                    {"B", -1 },
                                                                    {"F", -1 },
                                                                    {"M", -1 },
                                                                    {"S", -1 }
                                                                };
        private EventAggregator _eventAggregator;
        private readonly List<Tuple<string,string>> _actions = new List<Tuple<string, string>>()
                                                                {
                                                                    new Tuple<string, string>("Arrivato in laboratorio", "A"),
                                                                    new Tuple<string, string>("Buttato", "B"),
                                                                    new Tuple<string, string>("Finito", "F"),
                                                                    new Tuple<string, string>("Masterizzato", "M"),
                                                                    new Tuple<string, string>("Spedito", "S")
                                                                };
        private Sample _lastEntry;
        private string _batchNumber;
        private Tuple<string, string> _selectedAction;

        public SampleLogDialogViewModel(DBPrincipal principal,
                                        EventAggregator aggregator) : base()
        {
            _principal = principal;
            _eventAggregator = aggregator;

            _selectedAction = _actions.First(act => act.Item2 == "A");

            _confirm = new DelegateCommand(
                () =>
                {
                    Sample newLog = new Sample();
                    newLog.BatchID = _batchInstance.ID;
                    newLog.Code = _selectedAction.Item2;
                    newLog.Date = DateTime.Now.Date;
                    newLog.personID = _principal.CurrentPerson.ID;

                    newLog.Create();

                    if (_batchInstance.FirstSampleArrived == false)
                    {
                        _batchInstance.FirstSampleArrived = true;
                        _batchInstance.FirstSampleID = newLog.ID;
                    }

                    _batchInstance.ArchiveStock += _stockModifiers[newLog.Code];
                    _batchInstance.Update();

                    BatchNumber = null;

                    _eventAggregator.GetEvent<SampleLogCreated>()
                                    .Publish(newLog);

                    RaisePropertyChanged("LatestSampleList");
                },
                () => !HasErrors);

            _deleteLast = new DelegateCommand(
                () =>
                {

                });

            _end = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.Close();
                });

            BatchNumber = "";
        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion


        public IEnumerable<Tuple<string, string>> ActionList
        {
            get { return _actions; }
        }

        public Batch BatchInstance
        {
            get { return _batchInstance; }
            set
            {
                _batchInstance = value;

                if (_batchInstance == null)
                {
                    _validationErrors["BatchNumber"] = new List<string>() { "Batch inserito non valido" };
                }

                else if (_validationErrors.ContainsKey("BatchNumber"))
                    _validationErrors.Remove("BatchNumber");

                RaisePropertyChanged("BatchInstance");
                RaisePropertyChanged("MaterialCode");
            }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                BatchInstance = MaterialService.GetBatch(_batchNumber);

                RaiseErrorsChanged("BatchNumber");
                RaisePropertyChanged("BatchNumber");
            }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public DelegateCommand DeleteLastCommand
        {
            get { return _deleteLast; }
        }

        public DelegateCommand<Window> EndCommand
        {
            get { return _end; }
        }

        public IEnumerable<Sample> LatestSampleList
        {
            get { return MaterialService.GetLatestSamples(15); }
        }

        public string MaterialCode
        {
            get
            {
                if (_batchInstance == null || _batchInstance.Material == null)
                    return "";

                return _batchInstance.Material.MaterialType.Code
                        + _batchInstance.Material.MaterialLine.Code
                        + _batchInstance.Material.Aspect.Code
                        + _batchInstance.Material.Recipe.Code;
            }
        }

        public Tuple<string,string> SelectedAction
        {
            get { return _selectedAction; }
            set
            {
                _selectedAction = value;
            }
        }
    }
}
