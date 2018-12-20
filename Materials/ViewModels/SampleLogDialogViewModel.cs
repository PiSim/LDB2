using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Materials.ViewModels
{
    public class SampleLogDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private Batch _batchInstance;

        private string _batchNumber;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private MaterialService _materialService;

        #endregion Fields

        #region Constructors

        public SampleLogDialogViewModel(IDataService<LabDbEntities> labDbData,
                                        IEventAggregator aggregator,
                                        MaterialService materialService) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _materialService = materialService;

            SelectedChoice = SampleLogActions.ActionList.First(cho => cho.Code == "A");

            ConfirmCommand = new DelegateCommand(
                () =>
                {
                    Sample newLog = new Sample
                    {
                        BatchID = _batchInstance.ID,
                        Code = SelectedChoice.Code,
                        Date = DateTime.Now.Date,
                        personID = (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID
                    };

                    _labDbData.Execute(new InsertEntityCommand(newLog));

                    if (_batchInstance.FirstSampleArrived == false)
                    {
                        _batchInstance.FirstSampleArrived = true;
                        _batchInstance.FirstSampleID = newLog.ID;
                    }

                    if (newLog.Code == "A")
                        _batchInstance.LatestSampleID = newLog.ID;

                    _batchInstance.ArchiveStock += SelectedChoice.ArchiveModifier;
                    _batchInstance.LongTermStock += SelectedChoice.LongTermModifier;
                    _labDbData.Execute(new UpdateEntityCommand(_batchInstance));

                    BatchNumber = null;

                    _eventAggregator.GetEvent<SampleLogCreated>()
                                    .Publish(newLog);

                    RaisePropertyChanged("LatestSampleList");
                },
                () => !HasErrors);

            DeleteSampleCommand = new DelegateCommand<Sample>(
                smp =>
                {
                    _materialService.DeleteSample(smp);
                    RaisePropertyChanged("LatestSampleList");
                });

            EndCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.Close();
                });

            BatchNumber = "";
        }

        #endregion Constructors

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

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
                BatchInstance = _labDbData.RunQuery(new BatchQuery() { Number = _batchNumber });

                RaiseErrorsChanged("BatchNumber");
                RaisePropertyChanged("BatchNumber");
            }
        }

        public IEnumerable<SampleLogChoiceWrapper> ChoiceList => SampleLogActions.ActionList;
        public DelegateCommand ConfirmCommand { get; }

        public DelegateCommand<Sample> DeleteSampleCommand { get; }

        public DelegateCommand<Window> EndCommand { get; }

        public IEnumerable<Sample> LatestSampleList => _labDbData.RunQuery(new SamplesQuery() { OrderResults = true })
                                                                .Take(15)
                                                                .ToList();

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

        public SampleLogChoiceWrapper SelectedChoice { get; set; }

        #endregion Properties
    }
}