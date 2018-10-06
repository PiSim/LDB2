using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Materials.Commands;
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

namespace Materials.ViewModels
{
    public class BatchInfoViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private string _aspectCode,
                        _lineCode,
                        _recipeCode,
                        _typeCode;

        private Aspect _aspectInstance;
        private IEnumerable<Colour> _colourList;
        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private IEnumerable<ExternalConstruction> _externalConstructionList;
        private Batch _instance;
        private IDataService<LabDbEntities> _labDbData;
        private MaterialLine _lineInstance;
        private Material _materialInstance;
        private MaterialService _materialService;
        private IEnumerable<Project> _projectList;
        private Recipe _recipeInstance;
        private Colour _selectedColour;
        private ExternalConstruction _selectedExternalConstruction;
        private ExternalReport _selectedExternalReport;
        private Project _selectedProject;
        private Report _selectedReport;
        private TrialArea _selectedTrialArea;
        private MaterialType _typeInstance;

        #endregion Fields

        #region Constructors

        public BatchInfoViewModel(IEventAggregator aggregator,
                                IDataService<LabDbEntities> labDbData,
                                MaterialService materialService) : base()
        {
            _labDbData = labDbData;

            _materialService = materialService;
            _eventAggregator = aggregator;
            _editMode = false;

            TrialAreaList = _labDbData.RunQuery(new TrialAreasQuery())
                                    .ToList();

            CancelEditCommand = new DelegateCommand(
                () =>
                {
                    if (_instance == null)
                        BatchInstance = null;
                    else
                        BatchInstance = _labDbData.RunQuery(new BatchQuery()
                        {
                            ID = BatchInstance.ID
                        });
                },
                () => EditMode);

            DeleteBatchCommand = new DelegateCommand(
                () =>
                {
                    _instance.Delete();

                    BatchInstance = null;

                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchesView);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => CanDelete);

            DeleteSampleCommand = new DelegateCommand<Sample>(
                smp =>
                {
                    _materialService.DeleteSample(smp);
                    RaisePropertyChanged("Samples");
                },
                smp => Thread.CurrentPrincipal.IsInRole(UserRoleNames.BatchEdit));

            OpenExternalReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ExternalReportEditView,
                                                                _selectedExternalReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedExternalReport != null);

            OpenReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);

            RefreshCommand = new DelegateCommand(
                () =>
                {
                    BatchInstance = _labDbData.RunQuery(new BatchQuery()
                    {
                        ID = BatchInstance.ID
                    });
                },
                () => !EditMode);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    CreateChildEntitiesIfMissing();
                    ReloadChildEntitiesInstances();
                    Material _materialTemplate = GenerateMaterialUpdateTemplate();
                    _labDbData.Execute(new BatchUpdateCommand(BatchInstance, _materialTemplate));
                    EditMode = false;
                },
                () => EditMode && !HasErrors);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode && Thread.CurrentPrincipal.IsInRole(UserRoleNames.BatchEdit));

            #region EventSubscriptions

            _eventAggregator.GetEvent<ColorChanged>()
                            .Subscribe(ect =>
                            {
                                _colourList = null;
                                RaisePropertyChanged("ColourList");
                            });

            _eventAggregator.GetEvent<ExternalConstructionChanged>()
                .Subscribe(ect => ExternalConstructionChangedHandler());

            _eventAggregator.GetEvent<ExternalReportChanged>()
                .Subscribe(
                    ect =>
                    {
                        RaisePropertyChanged("ExternalReportList ");
                    });

            _eventAggregator.GetEvent<ProjectChanged>()
                .Subscribe(
                ect =>
                {
                    ProjectList = null;
                    SelectedProject = null;
                });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                });

            #endregion EventSubscriptions
        }


        #endregion Constructors

        #region Service method for internal use

        private void CheckMaterial()
        {
            // If all subEntities are not null checks to see if a corresponding material exists

            if (_typeInstance != null &&
                _lineInstance != null &&
                _aspectInstance != null &&
                _recipeInstance != null)
                MaterialInstance = _labDbData.RunQuery(new MaterialQuery()
                {
                    AspectID = _aspectInstance.ID,
                    MaterialLineID = _lineInstance.ID,
                    MaterialTypeID = _typeInstance.ID,
                    RecipeID = _recipeInstance.ID
                });
            else
                MaterialInstance = null;
        }

        private void CreateChildEntitiesIfMissing()
        {
            if (_typeInstance == null)
                _labDbData.Execute(new InsertEntityCommand(new MaterialType() { Code = TypeCode }));

            if (_lineInstance == null)
                _labDbData.Execute(new InsertEntityCommand(new MaterialLine() { Code = LineCode }));

            if (_aspectInstance == null)
                _labDbData.Execute(new InsertEntityCommand(new Aspect() { Code = AspectCode }));

            if (_recipeInstance == null)
                _labDbData.Execute(new InsertEntityCommand(new Recipe() { Code = RecipeCode }));
        }

        private void ExternalConstructionChangedHandler()
        {
            _externalConstructionList = null;
            RaisePropertyChanged("ExternalConstructionList");

            SelectedExternalConstruction = ExternalConstructionList.FirstOrDefault(exc => exc.ID == _instance?.Material?.ExternalConstructionID);
        }


        private void ReloadChildEntitiesInstances()
        {
            TypeCode = TypeCode;
            LineCode = LineCode;
            AspectCode = AspectCode;
            RecipeCode = RecipeCode;
        }

        /// <summary>
        /// Generates a template Material that will be passed to a BatchUpdateCommand instance to perform the Batch Update
        /// </summary>
        /// <returns>A disconnected Material instance containing the information required for the update</returns>
        private Material GenerateMaterialUpdateTemplate()
        {
            return new Material()
            {
                Aspect = new Aspect()
                {
                    Code = _aspectCode
                },

                ExternalConstruction = SelectedExternalConstruction,

                MaterialLine = new MaterialLine()
                {
                    Code = _lineCode
                },

                MaterialType = new MaterialType()
                {
                    Code = _typeCode
                },

                Project = SelectedProject,

                Recipe = new Recipe()
                {
                    Code = _recipeCode,
                    Colour = _selectedColour
                }
            };
        }

        #endregion Service method for internal use

        #region Events

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion Events

        #region Properties

        public bool HasErrors => _validationErrors.Count > 0;

        #endregion Properties

        #region Methods

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
            RaisePropertyChanged("HasErrors");
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion Methods

        #region Properties

        public string AspectCode
        {
            get { return _aspectCode; }
            set
            {
                _aspectCode = value;

                if (_aspectCode.Length == 3)
                {
                    AspectInstance = _labDbData.RunQuery(new AspectQuery()
                    {
                        AspectCode = _aspectCode
                    });

                    if (_validationErrors.ContainsKey("AspectCode"))
                    {
                        _validationErrors.Remove("AspectCode");
                        RaiseErrorsChanged("AspectCode");
                    }
                }
                else
                {
                    AspectInstance = null;
                    _validationErrors["AspectCode"] = new List<string>() { "Codice Aspetto non valido" };
                    RaiseErrorsChanged("AspectCode");
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
                CheckMaterial();
            }
        }

        public Batch BatchInstance
        {
            get { return _instance; }
            set
            {
                _validationErrors.Clear();
                EditMode = false;

                _instance = value;

                DeleteBatchCommand.RaiseCanExecuteChanged();

                _selectedTrialArea = TrialAreaList.FirstOrDefault(tra => tra.ID == _instance?.TrialAreaID);

                _typeInstance = _instance?.Material?.MaterialType;
                _typeCode = _typeInstance?.Code;

                _lineInstance = _instance?.Material?.MaterialLine;
                _lineCode = _lineInstance?.Code;

                _aspectInstance = _instance?.Material?.Aspect;
                _aspectCode = _aspectInstance?.Code;

                _recipeInstance = _instance?.Material?.Recipe;
                _recipeCode = _recipeInstance?.Code;

                _materialInstance = _instance?.Material;

                SelectedExternalConstruction = ExternalConstructionList.FirstOrDefault(excon => excon.ID == _materialInstance?.ExternalConstructionID);
                SelectedColour = ColourList.FirstOrDefault(col => col.ID == _recipeInstance?.ColourID);

                SelectedProject = ProjectList.FirstOrDefault(prj => prj.ID == _materialInstance?.ProjectID);

                SelectedExternalReport = null;
                SelectedReport = null;

                RaisePropertyChanged("TypeInstance");
                RaisePropertyChanged("TypeCode");
                RaisePropertyChanged("LineCode");
                RaisePropertyChanged("AspectInstance");
                RaisePropertyChanged("AspectCode");
                RaisePropertyChanged("RecipeCode");
                RaisePropertyChanged("DoNotTest");
                RaisePropertyChanged("ExternalReportList");
                RaisePropertyChanged("Samples");
                RaisePropertyChanged("Notes");
                RaisePropertyChanged("Number");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("SelectedTrialArea");
            }
        }

        public DelegateCommand CancelEditCommand { get; }

        public IEnumerable<Colour> ColourList
        {
            get
            {
                if (_colourList == null)
                    _colourList = _labDbData.RunQuery(new ColorsQuery())
                                            .ToList();
                return _colourList;
            }
        }

        public DelegateCommand DeleteBatchCommand { get; }

        public DelegateCommand<Sample> DeleteSampleCommand { get; }

        public bool DoNotTest
        {
            get { return (_instance == null) ? false : _instance.DoNotTest; }
            set
            {
                if (_instance == null)
                    return;

                _instance.DoNotTest = value;
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;

                RaisePropertyChanged("EditMode");
                CancelEditCommand.RaiseCanExecuteChanged();
                RefreshCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<ExternalConstruction> ExternalConstructionList
        {
            get
            {
                if (_externalConstructionList == null)
                    _externalConstructionList = _labDbData.RunQuery(new ExternalConstructionsQuery())
                                                        .ToList();

                return _externalConstructionList;
            }
        }

        public IEnumerable<ExternalReport> ExternalReportList => (_instance == null) ? null : _labDbData.RunQuery(new ExternalReportsQuery())
                                                                                                        .Where(exrep => exrep.TestRecords
                                                                                                        .Any(tstrec => tstrec.BatchID == _instance.ID))
                                                                                                        .ToList();
        
        public string LineCode
        {
            get { return _lineCode; }
            set
            {
                _lineCode = value;

                if (_lineCode.Length == 3)
                {
                    LineInstance = _labDbData.RunQuery(new MaterialLineQuery()
                    {
                        MaterialLineCode = LineCode
                    });
                    if (_validationErrors.ContainsKey("LineCode"))
                    {
                        _validationErrors.Remove("LineCode");
                        RaiseErrorsChanged("LineCode");
                    }
                }
                else
                {
                    LineInstance = null;
                    _validationErrors["LineCode"] = new List<string>() { "Codice Riga non valido" };
                    RaiseErrorsChanged("LineCode");
                }
            }
        }

        public MaterialLine LineInstance
        {
            get { return _lineInstance; }
            set
            {
                _lineInstance = value;
                CheckMaterial();
            }
        }

        public Material MaterialInstance
        {
            get
            {
                return _materialInstance;
            }
            set
            {
                _materialInstance = value;
                RaisePropertyChanged("ConstructionName");
            }
        }

        public string Notes
        {
            get { return _instance?.Notes; }
            set
            {
                _instance.Notes = value;
            }
        }

        public string Number => _instance?.Number;

        public DelegateCommand OpenExternalReportCommand { get; }

        public DelegateCommand OpenReportCommand { get; }

        public IEnumerable<Project> ProjectList
        {
            get => (_projectList != null) ? _projectList : _projectList = _labDbData.RunQuery(new ProjectsQuery()).ToList();
            private set => _projectList = value;
        }

        public string RecipeCode
        {
            get { return _recipeCode; }
            set
            {
                _recipeCode = value;

                if (_recipeCode.Length == 4)
                {
                    RecipeInstance = _labDbData.RunQuery(new RecipeQuery()
                    {
                        RecipeCode = _recipeCode
                    });

                    if (_validationErrors.ContainsKey("RecipeCode"))
                    {
                        _validationErrors.Remove("RecipeCode");
                        RaiseErrorsChanged("RecipeCode");
                    }
                }
                else
                {
                    RecipeInstance = null;
                    _validationErrors["RecipeCode"] = new List<string>() { "Codice colore non valido" };
                }
            }
        }

        public Recipe RecipeInstance
        {
            get { return _recipeInstance; }
            set
            {
                _recipeInstance = value;
                CheckMaterial();

                SelectedColour = _colourList.FirstOrDefault(col => col.ID == _recipeInstance?.ColourID);

                RaisePropertyChanged("SelectedColour");
            }
        }

        public DelegateCommand RefreshCommand { get; }

        public IEnumerable<Report> ReportList => _instance?.Reports;

        public IEnumerable<Sample> Samples => _instance?.Samples;

        public DelegateCommand SaveCommand { get; }

        public Colour SelectedColour
        {
            get { return _selectedColour; }
            set
            {
                _selectedColour = value;
                RaisePropertyChanged("SelectedColour");
            }
        }

        public ExternalConstruction SelectedExternalConstruction
        {
            get
            {
                return _selectedExternalConstruction;
            }
            set
            {
                _selectedExternalConstruction = value;
                RaisePropertyChanged("SelectedExternalConstruction");
            }
        }

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedExternalReport; }
            set
            {
                _selectedExternalReport = value;
                OpenExternalReportCommand.RaiseCanExecuteChanged();
            }
        }

        public Project SelectedProject
        {
            get => _selectedProject;

            set
            {
                _selectedProject = value;
                RaisePropertyChanged("SelectedProject");
            }
        }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OpenReportCommand.RaiseCanExecuteChanged();
            }
        }

        public TrialArea SelectedTrialArea
        {
            get { return _selectedTrialArea; }
            set
            {
                _selectedTrialArea = value;
                _instance.TrialAreaID = _selectedTrialArea.ID;
            }
        }

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<TrialArea> TrialAreaList { get; }

        public string TypeCode
        {
            get { return _typeCode; }
            set
            {
                _typeCode = value;

                if (_typeCode.Length == 4)
                {
                    TypeInstance = _labDbData.RunQuery(new MaterialTypeQuery()
                    {
                        MaterialTypeCode = _typeCode
                    });

                    if (_validationErrors.ContainsKey("TypeCode"))
                    {
                        _validationErrors.Remove("TypeCode");
                        RaiseErrorsChanged("TypeCode");
                    }
                }
                else
                    TypeInstance = null;

                if (_typeInstance == null)
                {
                    _validationErrors["TypeCode"] = new List<string>() { "Codice Tipo non Valido" };
                    RaiseErrorsChanged("TypeCode");
                }
            }
        }

        public MaterialType TypeInstance
        {
            get { return _typeInstance; }

            set
            {
                _typeInstance = value;
                RaisePropertyChanged("TypeInstance");
                CheckMaterial();
            }
        }

        private bool CanDelete => _instance != null
                                                                                                                                                                                                                                                                                            && Thread.CurrentPrincipal.IsInRole(UserRoleNames.BatchEdit)
                                    && !_instance.HasTests;

        #endregion Properties
    }
}