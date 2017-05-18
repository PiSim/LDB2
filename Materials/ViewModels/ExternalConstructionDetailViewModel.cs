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
    public class ExternalConstructionDetailViewModel : BindableBase 
    {
        private bool _isEditMode;
        private Construction _selectedAssignedConstruction, _selectedUnassignedConstruction;
        private DBEntities _entities;
        private DelegateCommand _assignConstructionToExternal, _setModify, _unassignConstructionToExternal;
        private EventAggregator _eventAggregator;
        private ExternalConstruction _externalConstructionInstance;
        private Specification _selectedSpecification;

        public ExternalConstructionDetailViewModel(DBEntities entities,
                                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _isEditMode = false;

            _assignConstructionToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.Constructions.Add(_selectedAssignedConstruction);
                    SelectedUnassignedconstruction = null;
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedUnassignedConstruction != null && _externalConstructionInstance != null);

            _setModify = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                });

            _unassignConstructionToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.Constructions.Remove(_selectedAssignedConstruction);
                    SelectedAssignedConstruction = null;
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedAssignedConstruction != null);

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(
                () =>
                {
                    if (!_isEditMode)
                        return;
                    else
                    {
                        _entities.SaveChanges();
                        EditMode = false;
                    }
                });
        }

        public DelegateCommand AssignConstructionToExternalCommand
        {
            get { return _assignConstructionToExternal; }
        }

        public List<Construction> AssignedConstructions
        {
            get
            {
                return new List<Construction>(_externalConstructionInstance.Constructions);
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
                    _externalConstructionInstance = _entities.ExternalConstructions.First(exc => exc.ID == value.ID);
                    _selectedSpecification = _externalConstructionInstance.DefaultSpecVersion.Specification;
                }       

                SelectedAssignedConstruction = null;
                SelectedUnassignedconstruction = null;

                RaisePropertyChanged("AssignedConstructions");
                RaisePropertyChanged("ExternalConstructionInstance");
                RaisePropertyChanged("ExternalConstructionSpecificationVersion");
                RaisePropertyChanged("ExternalConstructionName");
                RaisePropertyChanged("SelectedOEM");
                RaisePropertyChanged("SelectedSpecification");
            }
        }

        public string ExternalConstructionName
        {
            get { return _externalConstructionInstance.Name; }
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
                if (_externalConstructionInstance == null)
                    return null;
                else
                    return _externalConstructionInstance.DefaultSpecVersion;
            }
            set
            {
                _externalConstructionInstance.DefaultSpecVersion = value;
            }
        }

        public List<Organization> OEMList
        {
            get
            {
                OrganizationRole tempRole = _entities.OrganizationRoles.First
                    (orgr => orgr.Name == OrganizationRoleNames.OEM);

                return new List<Organization>(tempRole.OrganizationMappings.Where(orm => orm.IsSelected)
                                                                            .Select(orm => orm.Organization)
                                                                            .OrderBy(org => org.Name));
            }
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
                    return OEMList.First();
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
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                ExternalConstructionSpecificationVersion = _selectedSpecification.SpecificationVersions.FirstOrDefault(spcv => spcv.IsMain);

                RaisePropertyChanged("EnableVersionSelection");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("SpecificationVersionList");
            }
        }

        public Construction SelectedUnassignedconstruction
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

        public List<Specification> SpecificationList
        {
            get { return new List<Specification>(_entities.Specifications.OrderBy(spec => spec.Standard.Name)); }
        }

        public List<SpecificationVersion> SpecificationVersionList
        {
            get
            {
                if (_selectedSpecification == null)
                    return null;

                return new List<SpecificationVersion>(_selectedSpecification.SpecificationVersions);
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
                return new List<Construction>(_entities.Constructions.Where(cns => cns.ExternalConstruction == null));
            }
        }
    }
}
