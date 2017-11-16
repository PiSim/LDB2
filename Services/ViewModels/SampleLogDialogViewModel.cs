using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
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
        private DelegateCommand _confirm;
        private DelegateCommand<Sample> _deleteSample;
        private DelegateCommand<Window> _end;

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;

        private string _batchNumber;
        private SampleLogChoiceWrapper _selectedChoice;

        public SampleLogDialogViewModel(DBPrincipal principal,
                                        EventAggregator aggregator) : base()
        {
            _principal = principal;
            _eventAggregator = aggregator;

            _selectedChoice = SampleLogActions.ActionList.First(cho => cho.Code == "A");

            _confirm = new DelegateCommand(
                () =>
                {
                    Sample newLog = new Sample
                    {
                        BatchID = _batchInstance.ID,
                        Code = _selectedChoice.Code,
                        Date = DateTime.Now.Date,
                        personID = _principal.CurrentPerson.ID
                    };

                    newLog.Create();

                    if (_batchInstance.FirstSampleArrived == false)
                    {
                        _batchInstance.FirstSampleArrived = true;
                        _batchInstance.FirstSampleID = newLog.ID;
                    }

                    if (newLog.Code == "A")
                        _batchInstance.LatestSampleID = newLog.ID;

                    _batchInstance.ArchiveStock += _selectedChoice.ArchiveModifier;
                    _batchInstance.LongTermStock += _selectedChoice.LongTermModifier;
                    _batchInstance.Update();

                    BatchNumber = null;

                    _eventAggregator.GetEvent<SampleLogCreated>()
                                    .Publish(newLog);

                    RaisePropertyChanged("LatestSampleList");
                },
                () => !HasErrors);

            _deleteSample = new DelegateCommand<Sample>(
                smp =>
                {
                    CommonProcedures.DeleteSample(smp);
                    RaisePropertyChanged("LatestSampleList");
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


        public IEnumerable<SampleLogChoiceWrapper> ChoiceList
        {
            get { return SampleLogActions.ActionList; }
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

        public DelegateCommand<Sample> DeleteSampleCommand
        {
            get { return _deleteSample; }
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

        public SampleLogChoiceWrapper SelectedChoice
        {
            get { return _selectedChoice; }
            set
            {
                _selectedChoice = value;
            }
        }
    }
}
