using DBManager;
using DBManager.Services;
using Microsoft.Practices.Unity;
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
    public class ExternalConstructionDetailViewModel : BindableBase 
    {
        private bool _isEditMode;
        private Construction _selectedAssignedConstruction, _selectedUnassignedConstruction;
        private DBPrincipal _principal;
        private DelegateCommand _assignConstructionToExternal, _setModify, _unassignConstructionToExternal;
        private EventAggregator _eventAggregator;
        private ExternalConstruction _externalConstructionInstance;
        private IEnumerable<Organization> _oemList;
        private IEnumerable<Specification> _specificationList;
        private Specification _selectedSpecification;

        public ExternalConstructionDetailViewModel(DBPrincipal principal,
                                                    EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _principal = principal;
            _isEditMode = false;

            _oemList = new List<Organization>(DataService.GetOEMs());
            _specificationList = new List<Specification>(SpecificationService.GetSpecifications());

            _assignConstructionToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.AddConstruction(_selectedUnassignedConstruction);
                    SelectedUnassignedConstruction = null;
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedUnassignedConstruction != null 
                    && _externalConstructionInstance != null
                    && !EditMode
                    && CanModify);

            _setModify = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanModify);

            _unassignConstructionToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.RemoveConstruction(_selectedAssignedConstruction);
                    SelectedAssignedConstruction = null;
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedAssignedConstruction != null
                    && CanModify
                    && !EditMode);

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(
                () =>
                {
                    if (!_isEditMode)
                        return;
                    else
                    {
                        _externalConstructionInstance.Update();
                        EditMode = false;
                    }
                });
        }

        public DelegateCommand AssignConstructionToExternalCommand
        {
            get { return _assignConstructionToExternal; }
        }

        public IEnumerable<Construction> AssignedConstructions
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return new List<Construction>();

                else
                    return new List<Construction>(_externalConstructionInstance.Constructions);
            }
        }

        public IEnumerable<Batch> BatchList
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return new List<Batch>();

                else
                    return new List<Batch>(DataService.GetBatchesForExternalConstruction(_externalConstructionInstance));
            }
        }

        public bool CanModify
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.MaterialAdmin);
            }
        }

        public bool EditMode
        {
            get { return _isEditMode; }
            set
            {
                _isEditMode = value;

                _assignConstructionToExternal.RaiseCanExecuteChanged();
                _unassignConstructionToExternal.RaiseCanExecuteChanged();
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("EnableVersionSelection");
            }
        }

        public bool EnableVersionSelection
        {
            get { return _selectedSpecification != null; }
        }

        public string ExternalConstructionBatchListRegionName
        {
            get { return RegionNames.ExternalConstructionBatchListRegion; }
        }

        public ExternalConstruction ExternalConstructionInstance
        {
            get { return _externalConstructionInstance; }
            set
            {
                if (value == null)
                {
                    _externalConstructionInstance = value;
                    _selectedSpecification = null;
                }
                else
                {
                    _externalConstructionInstance = value;
                    _externalConstructionInstance.Load();

                    if (_externalConstructionInstance.DefaultSpecVersion != null)
                    {
                        _selectedSpecification = _specificationList.First(spec => spec.ID == _externalConstructionInstance.DefaultSpecVersion.Specification.ID);
                        _selectedSpecification.Load();
                    }
                }       

                SelectedAssignedConstruction = null;
                SelectedUnassignedConstruction = null;

                EditMode = false;

                RaisePropertyChanged("AssignedConstructions");
                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("ExternalConstructionInstance");
                RaisePropertyChanged("ExternalConstructionName");
                RaisePropertyChanged("SelectedOEM");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("SpecificationVersionList");
                RaisePropertyChanged("ExternalConstructionSpecificationVersion");
            }
        }

        public string ExternalConstructionName
        {
            get
            {
                if (_externalConstructionInstance == null)
                   return null;

                else
                    return _externalConstructionInstance.Name;
            }
            set
            {
                _externalConstructionInstance.Name = value;

                RaisePropertyChanged("ExternalConstructionName");
            }
        }

        public SpecificationVersion ExternalConstructionSpecificationVersion
        {
            get
            {
                if (_externalConstructionInstance == null || _externalConstructionInstance.DefaultSpecVersion == null)
                    return null;
                else
                    return SpecificationVersionList.FirstOrDefault(spcv => spcv.ID == _externalConstructionInstance.DefaultSpecVersion.ID);
            }
            set
            {
                _externalConstructionInstance.DefaultSpecVersion = value;
                _externalConstructionInstance.DefaultSpecVersionID = (value == null) ? 0 : value.ID;
            }
        }

        public IEnumerable<Organization> OEMList
        {
            get
            {
                return _oemList;
            }
        }

        private void RaiseExternalConstructionModified()
        {
            _eventAggregator.GetEvent<ExternalConstructionModified>().Publish();
        }

        public Construction SelectedAssignedConstruction
        {
            get { return _selectedAssignedConstruction; }
            set
            {
                _selectedAssignedConstruction = value;
                _unassignConstructionToExternal.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedAssignedConstruction");
            }
        }

        public Organization SelectedOEM
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return null;
                else
                    return _oemList.FirstOrDefault(org => org.ID == _externalConstructionInstance.Organization.ID);
            }
            set
            {
                if (_externalConstructionInstance == null)
                    return;
                else
                    _externalConstructionInstance.Organization = value;

                RaisePropertyChanged("SelectedOEM");
            }
        }

        public Specification SelectedSpecification
        {
            get
            {
                if (_externalConstructionInstance == null || _externalConstructionInstance.DefaultSpecVersion == null)
                    return null;
                else
                    return _selectedSpecification;
            }
            set
            {
                _selectedSpecification = value;
                _selectedSpecification.Load();
                ExternalConstructionSpecificationVersion = _selectedSpecification.SpecificationVersions.FirstOrDefault(spcv => spcv.IsMain);

                RaisePropertyChanged("EnableVersionSelection");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("SpecificationVersionList");
            }
        }

        public Construction SelectedUnassignedConstruction
        {
            get { return _selectedUnassignedConstruction; }
            set
            {
                _selectedUnassignedConstruction = value;
                _assignConstructionToExternal.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedUnassignedConstruction");
            }
        }

        public DelegateCommand SetModifyCommand
        {
            get { return _setModify; }
        }

        public IEnumerable<Specification> SpecificationList
        {
            get { return _specificationList; }
        }

        public IEnumerable<SpecificationVersion> SpecificationVersionList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<SpecificationVersion>();
                
                else
                    return _selectedSpecification.SpecificationVersions;
            }
        }

        public DelegateCommand UnassignConstructionToExternalCommand
        {
            get { return _unassignConstructionToExternal; }
        }

        public List<Construction> UnassignedConstructions
        {
            get
            {
                return new List<Construction>(DataService.GetConstructionsWithoutExternal());
            }
        }
    }
}
