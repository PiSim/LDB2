using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Materials.ViewModels
{
    public class AspectDetailViewModel : BindableBase
    {
        #region Fields

        private Aspect _aspectInstance;
        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Batch _selectedBatch;

        #endregion Fields

        #region Constructors

        public AspectDetailViewModel(IDataService<LabDbEntities> labDbData,
                                    IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _editMode = false;
            _labDbData = labDbData;

            OpenBatchCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => _selectedBatch != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    EditMode = false;
                    _labDbData.Execute(new UpdateEntityCommand(_aspectInstance));
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanSetModify && !_editMode);
        }

        #endregion Constructors

        #region Properties

        public static string AspectDetailBatchListRegionName => RegionNames.AspectDetailBatchListRegion;

        public string AspectCode
        {
            get
            {
                if (_aspectInstance == null)
                    return null;
                else
                    return _aspectInstance.Code;
            }
        }

        public Aspect AspectInstance
        {
            get { return _aspectInstance; }
            set
            {
                _aspectInstance = value;
                RaisePropertyChanged("AspectInstance");
                RaisePropertyChanged("AspectCode");
                RaisePropertyChanged("AspectName");
                RaisePropertyChanged("BatchList");
            }
        }

        public string AspectName
        {
            get
            {
                if (_aspectInstance == null)
                    return null;
                else
                    return _aspectInstance.Name;
            }

            set
            {
                if (_aspectInstance == null)
                    return;
                else
                {
                    _aspectInstance.Name = value;
                    RaisePropertyChanged("AspectName");
                }
            }
        }

        public IEnumerable<Batch> BatchList => (_aspectInstance == null ) ? null : _labDbData.RunQuery(new BatchesQuery()).Where(btc => btc.Material.Aspect.ID == _aspectInstance.ID).ToList();

        public bool CanSetModify => true;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OpenBatchCommand { get; }

        public DelegateCommand SaveCommand { get; }

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                OpenBatchCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand StartEditCommand { get; }

        #endregion Properties
    }
}