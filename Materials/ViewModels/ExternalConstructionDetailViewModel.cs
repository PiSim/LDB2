using DBManager;
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
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _assignConstructionToExternal, _setModify, _unassignConstructionToExternal;
        private EventAggregator _eventAggregator;
        private ExternalConstruction _externalConstructionInstance;
        private Specification _selectedSpecification;

        public ExternalConstructionDetailViewModel(DBEntities entities,
                                                    DBPrincipal principal,
                                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;
            _isEditMode = false;

            _assignConstructionToExternal = new DelegateCommand(
                () =>
                {
                    
                    _externalConstructionInstance.Constructions.Add(_selectedUnassignedConstruction);
                    SelectedUnassignedConstruction = null;
                    _entities.SaveChanges();
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedUnassignedConstruction != null 
                    && _externalConstructionInstance != null
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
                    _externalConstructionInstance.Constructions.Remove(_selectedAssignedConstruction);
                    SelectedAssignedConstruction = null;
                    _entities.SaveChanges();
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedAssignedConstruction != null
                    && CanModify);

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(
                () =>
                {
                    if (!_isEditMode)
                        return;
                    else
                    {
                        _entities.Database.CurrentTransaction.Commit();
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
                if (_externalConstructionInstance == null)
                    return new List<Construction>();

                else
                    return new List<Construction>(_externalConstructionInstance.Constructions);
            }
        }

        public List<Batch> BatchList
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return new List<Batch>();

                else
                    return new List<Batch>(_entities.Batches
                        .Where(btc => btc.Material.Construction.ExternalConstruction.ID == _externalConstructionInstance.ID));
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

                if (value)
                    _entities.Database.BeginTransaction();

                else if (_entities.Database.CurrentTransaction != null)
                    _entities.Database.CurrentTransaction.Dispose();

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
                    _externalConstructionInstance = _entities.ExternalConstructions.First(exc => exc.ID == value.ID);

                    _selectedSpecification = (_externalConstructionInstance.DefaultSpecVersion != null) ?
                        _externalConstructionInstance.DefaultSpecVersion.Specification : null;
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
                    return _externalConstructionInstance.Organization;
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

        public List<Specification> SpecificationList
        {
            get { return new List<Specification>(_entities.Specifications.OrderBy(spec => spec.Standard.Name)); }
        }

        public List<SpecificationVersion> SpecificationVersionList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<SpecificationVersion>();
                
                else
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
