using DBManager;
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
        private DBEntities _entities;
        private DelegateCommand _openBatch,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;

        public AspectDetailViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
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
                    _entities.SaveChanges();
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
                _aspectInstance = _entities.Aspects.First(asp => asp.ID == value.ID);
                RaisePropertyChanged("AspectInstance");
                RaisePropertyChanged("AspectCode");
                RaisePropertyChanged("AspectName");
                RaisePropertyChanged("BatchList");
            }
        }

        public List<Batch> BatchList
        {
            get
            {
                if (_aspectInstance == null)
                    return new List<Batch>();

                else
                    return new List<Batch>(_entities.Batches.Where(btc => btc.Material.AspectID == _aspectInstance.ID));
            }
        }

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
