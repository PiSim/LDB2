using DBManager;
using DBManager.EntityExtensions;
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
using DBManager.EntityExtensions;

namespace Materials.ViewModels
{
    public class ExternalConstructionDetailViewModel : BindableBase 
    {
        private bool _editMode;
        private Material _selectedAssignedMaterial, _selectedUnassignedMaterial;
        private DBPrincipal _principal;
        private DelegateCommand _assignMaterialToExternal, 
                                _save,
                                _startEdit,
                                _unassignMaterialToExternal;
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
            _editMode = false;

            _oemList = new List<Organization>(OrganizationService.GetOrganizations(OrganizationRoleNames.OEM));
            _specificationList = new List<Specification>(SpecificationService.GetSpecifications());

            _assignMaterialToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.AddMaterial(_selectedUnassignedMaterial);
                    SelectedUnassignedMaterial = null;
                    RaisePropertyChanged("AssignedMaterials");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedMaterials");
                },
                () => _selectedUnassignedMaterial != null 
                    && _externalConstructionInstance != null
                    && !EditMode
                    && CanModify);

            _save = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.Update();
                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanModify && !_editMode);

            _unassignMaterialToExternal = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.RemoveMaterial(_selectedAssignedMaterial);
                    SelectedAssignedMaterial = null;
                    RaisePropertyChanged("AssignedMaterials");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedMaterials");
                },
                () => _selectedAssignedMaterial != null
                    && CanModify
                    && !EditMode);
        }

        public DelegateCommand AssignMaterialToExternalCommand
        {
            get { return _assignMaterialToExternal; }
        }

        public IEnumerable<Material> AssignedMaterials
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return new List<Material>();

                else
                    return new List<Material>(_externalConstructionInstance.GetMaterials());
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
            get { return _editMode; }
            set
            {
                _editMode = value;

                _assignMaterialToExternal.RaiseCanExecuteChanged();
                _unassignMaterialToExternal.RaiseCanExecuteChanged();
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
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

                SelectedAssignedMaterial = null;
                SelectedUnassignedMaterial = null;

                EditMode = false;

                RaisePropertyChanged("AssignedMaterials");
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

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public Material SelectedAssignedMaterial
        {
            get { return _selectedAssignedMaterial; }
            set
            {
                _selectedAssignedMaterial = value;
                _unassignMaterialToExternal.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedAssignedMaterial");
            }
        }

        public Organization SelectedOEM
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return null;
                else
                    return _oemList.FirstOrDefault(org => org.ID == _externalConstructionInstance.OemID);
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

        public Material SelectedUnassignedMaterial
        {
            get { return _selectedUnassignedMaterial; }
            set
            {
                _selectedUnassignedMaterial = value;
                _assignMaterialToExternal.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedUnassignedMaterial");
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
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

        public DelegateCommand UnassignMaterialToExternalCommand
        {
            get { return _unassignMaterialToExternal; }
        }

        public List<Material> UnassignedMaterials
        {
            get
            {
                return new List<Material>(DataService.GetMaterialsWithoutConstruction());
            }
        }
    }
}
