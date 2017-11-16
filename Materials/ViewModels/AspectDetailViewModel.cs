using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
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
    public class AspectDetailViewModel : BindableBase
    {
        private Aspect _aspectInstance;
        private Batch _selectedBatch;
        private bool _editMode;
        private DelegateCommand _openBatch,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;

        public AspectDetailViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _editMode = false;

            _openBatch = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => _selectedBatch != null);

            _save = new DelegateCommand(
                () =>
                {
                    EditMode = false;
                    _aspectInstance.Update();
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanSetModify && !_editMode);
        }

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

        public static string AspectDetailBatchListRegionName
        {
            get { return RegionNames.AspectDetailBatchListRegion; }
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

        public IEnumerable<Batch> BatchList => _aspectInstance?.GetBatches();

        public bool CanSetModify
        {
            get { return true; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                _openBatch.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
