using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Specifications.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Materials.ViewModels
{
    public class ExternalConstructionDetailViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private ExternalConstruction _externalConstructionInstance;
        private IDataService<LabDbEntities> _labDbData;
        private IReportService _reportService;
        private Material _selectedAssignedMaterial, _selectedUnassignedMaterial;
        private Specification _selectedSpecification;
        private DelegateCommand _unassignMaterialToExternal;

        #endregion Fields

        #region Constructors

        public ExternalConstructionDetailViewModel(IDataService<LabDbEntities> labDbdata,
                                                    IEventAggregator eventAggregator,
                                                    IReportService reportService) : base()
        {
            _labDbData = labDbdata;
            _reportService = reportService;
            _eventAggregator = eventAggregator;
            _editMode = false;

            OEMList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.OEM }).ToList(); ;
            SpecificationList = _labDbData.RunQuery(new SpecificationsQuery()).ToList();

            AssignMaterialToExternalCommand = new DelegateCommand(
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

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _externalConstructionInstance.Update();
                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => CanModify && !_editMode);

            _unassignMaterialToExternal = new DelegateCommand(
                () =>
                {
                    _selectedAssignedMaterial.UnsetConstruction();
                    SelectedAssignedMaterial = null;
                    RaisePropertyChanged("AssignedMaterials");
                    RaisePropertyChanged("BatchList");
                    RaisePropertyChanged("UnassignedMaterials");
                },
                () => _selectedAssignedMaterial != null
                    && CanModify
                    && !EditMode);
        }

        #endregion Constructors

        #region Properties

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

        public DelegateCommand AssignMaterialToExternalCommand { get; }

        public IEnumerable<Batch> BatchList
        {
            get
            {
                if (_externalConstructionInstance == null)
                    return new List<Batch>();
                else
                {
                    BatchesQuery query = new BatchesQuery()
                    {
                        ExternalConstructionID = _externalConstructionInstance.ID
                    };

                    return _labDbData.RunQuery(query).ToList();
                }
            }
        }

        public bool CanModify => Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialEdit);

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;

                AssignMaterialToExternalCommand.RaiseCanExecuteChanged();
                _unassignMaterialToExternal.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("EnableVersionSelection");
            }
        }

        public bool EnableVersionSelection => _selectedSpecification != null;

        public string ExternalConstructionBatchListRegionName => RegionNames.ExternalConstructionBatchListRegion;

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

                    if (_externalConstructionInstance.DefaultSpecVersion != null)
                    {
                        _selectedSpecification = SpecificationList.First(spec => spec.ID == _externalConstructionInstance.DefaultSpecVersion.Specification.ID);
                        SpecificationVersionList = _selectedSpecification.GetVersions();
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
                RaisePropertyChanged("ExternalConstructionSpecificationVersion");
                RaisePropertyChanged("SpecificationVersionList");
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
                    return SpecificationVersionList?.FirstOrDefault(spcv => spcv.ID == _externalConstructionInstance.DefaultSpecVersionID);
            }
            set
            {
                _externalConstructionInstance.DefaultSpecVersion = value;
                _externalConstructionInstance.DefaultSpecVersionID = (value == null) ? (int?)null : value.ID;
            }
        }

        public IEnumerable<Organization> OEMList { get; }

        public DelegateCommand SaveCommand { get; }

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
                    return OEMList.FirstOrDefault(org => org.ID == _externalConstructionInstance.OemID);
            }
            set
            {
                if (_externalConstructionInstance == null)
                    return;
                else
                    _externalConstructionInstance.OemID = value.ID;

                RaisePropertyChanged("SelectedOEM");
            }
        }

        public Specification SelectedSpecification
        {
            get
            {
                return _selectedSpecification;
            }
            set
            {
                _selectedSpecification = value;

                SpecificationVersionList = _selectedSpecification.GetVersions();
                ExternalConstructionSpecificationVersion = SpecificationVersionList.First(spcv => spcv.IsMain);

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
                AssignMaterialToExternalCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedUnassignedMaterial");
            }
        }

        public IEnumerable<Specification> SpecificationList { get; }

        public IEnumerable<SpecificationVersion> SpecificationVersionList { get; private set; }

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<Material> UnassignedMaterials => _labDbData.RunQuery(new MaterialsQuery())
                                                                    .Where(mat => mat.ExternalConstructionID == null)
                                                                    .ToList();

        public DelegateCommand UnassignMaterialToExternalCommand => _unassignMaterialToExternal;

        #endregion Properties

        #region Methods

        private void RaiseExternalConstructionChanged()
        {
            EntityChangedToken entityChangedToken = new EntityChangedToken(_externalConstructionInstance,
                                                                            EntityChangedToken.EntityChangedAction.Updated);

            _eventAggregator.GetEvent<ExternalConstructionChanged>()
                            .Publish(entityChangedToken);
        }

        #endregion Methods
    }
}