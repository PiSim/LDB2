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
        private bool _editMode;
        private DBEntities _entities;
        private DelegateCommand _setModify;
        private EventAggregator _eventAggregator;

        public AspectDetailViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _editMode = false;

            _setModify = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanSetModify);

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(
                () =>
                {
                    if (_editMode)
                    {
                        EditMode = false;
                        _entities.SaveChanges();
                    }
                    else
                        return;
                });
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
                    return new List<Batch>(_entities.Batches.Where(btc => btc.Material.Construction.Aspect.ID == _aspectInstance.ID));
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
            }
        }

        public DelegateCommand SetModifyCommand
        {
            get { return _setModify; }
        }
    }
}
