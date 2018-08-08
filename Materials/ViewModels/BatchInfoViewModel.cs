using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class BatchInfoViewModel : BindableBase, INotifyDataErrorInfo
    {
        private Aspect _aspectInstance;
        private Batch _instance;
        private bool _editMode;
        private Colour _selectedColour;
        private DBPrincipal _principal;
        private DelegateCommand _cancelEdit,
                                _deleteBatch,
                                _openExternalReport, 
                                _openReport,
                                _refresh,
                                _save,
                                _startEdit;
        private DelegateCommand<Sample> _deleteSample;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternalReport;
        private ExternalConstruction _selectedExternalConstruction;
        private IDataService _dataService;
        private IEnumerable<Colour> _colourList;
        private IEnumerable<ExternalConstruction> _externalConstructionList;
        private IEnumerable<Project> _projectList;
        private IEnumerable<TrialArea> _trialAreaList; 
        private IEnumerable<Sample> _samplesList;
        private IMaterialService _materialService;
        private Material _materialInstance;
        private MaterialLine _lineInstance;
        private Project _selectedProject;
        private Recipe _recipeInstance;
        private Report _selectedReport;
        private string _aspectCode,
                        _lineCode,
                        _recipeCode,
                        _typeCode;
        private MaterialType _typeInstance;
        private TrialArea _selectedTrialArea;

        public BatchInfoViewModel(DBPrincipal principal,
                                EventAggregator aggregator,
                                IDataService dataService,
                                IMaterialService materialService) : base()
        {
            _dataService = dataService;
            _materialService = materialService;
            _eventAggregator = aggregator;
            _principal = principal;
            _editMode = false;

            _trialAreaList = _dataService.GetTrialAreas();

            _cancelEdit = new DelegateCommand(
                () =>
                {
                    if (_instance == null)
                        BatchInstance = null;
                    else
                        BatchInstance = _dataService.GetBatch(_instance.ID);
                },
                () => EditMode);

            _deleteBatch = new DelegateCommand(
                () =>
                {
                    _instance.Delete();

                    BatchInstance = null;

                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchesView);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                },
                () => CanDelete);

            _deleteSample = new DelegateCommand<Sample>(
                smp =>
                {
                    _materialService.DeleteSample(smp);
                    _samplesList = _instance.GetSamples();
                    RaisePropertyChanged("Samples");
                },
                smp => _principal.IsInRole(UserRoleNames.BatchEdit));

            _openExternalReport = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ExternalReportEditView,
                                                                _selectedExternalReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedExternalReport != null);

            _openReport = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ReportViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);

            _refresh = new DelegateCommand(
                () =>
                {
                    BatchInstance = _dataService.GetBatch(_instance.ID);
                },
                () => !EditMode);

            _save = new DelegateCommand(
                () =>
                {
                    CheckMaterial();
                    
                    // If a material with the given parameters does not exist, create it
                    // Then update the batch instance with the correct ID

                    if (_materialInstance == null)
                        _materialInstance = _materialService.CreateMaterial(_typeCode,
                                                                            _lineCode,
                                                                            _aspectCode,
                                                                            _recipeCode);

                    _instance.MaterialID = _materialInstance.ID;

                    // If construction and/or project are selected and are different from
                    // the one in the material instance, update

                    bool requiresUpdate = false;

                    if (_selectedExternalConstruction != null
                        && _selectedExternalConstruction.ID != _materialInstance.ExternalConstructionID)
                    {
                        _materialInstance.ExternalConstructionID = _selectedExternalConstruction.ID;
                        requiresUpdate = true;
                    }

                    if (_selectedProject != null
                        && _selectedProject.ID != _materialInstance.ProjectID)
                    {
                        _materialInstance.ProjectID = _selectedProject.ID;
                        requiresUpdate = true;
                    }

                    if (requiresUpdate)
                        _materialInstance.Update();

                    // If color is selected retrieve updated Recipe instance
                    // If the Recipe ColorID is different from the one selected, update

                    if (_selectedColour != null)
                    {
                        Recipe _recipeInstance = _dataService.GetRecipe(_materialInstance.RecipeID);

                        if (_recipeInstance.ColourID != _selectedColour.ID)
                        {
                            _recipeInstance.ColourID = _selectedColour.ID;
                            _recipeInstance.Update();
                        }
                    }


                    _instance.Update();

                    EditMode = false;
                },
                () => EditMode && !HasErrors);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode && _principal.IsInRole(UserRoleNames.BatchEdit));

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

            #endregion
        }

        #region Service method for internal use

        private void CheckMaterial()
        {
            // If all subEntities are not null checks to see if a corresponding material exists

            if (_typeInstance != null &&
                _lineInstance != null &&
                _aspectInstance != null &&
                _recipeInstance != null)
                MaterialInstance = _dataService.GetMaterial(_typeInstance,
                                                            _lineInstance,
                                                            _aspectInstance,
                                                            _recipeInstance);
            else
                MaterialInstance = null;
        }

        private void ExternalConstructionChangedHandler()
        {
            _externalConstructionList = null;
            RaisePropertyChanged("ExternalConstructionList");

            SelectedExternalConstruction = ExternalConstructionList.FirstOrDefault(exc => exc.ID == _instance?.Material?.ExternalConstructionID);
        }

        #endregion

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
            _save.RaiseCanExecuteChanged();
        }

        #endregion

        public string AspectCode
        {
            get { return _aspectCode; }
            set
            {
                _aspectCode = value;

                if (_aspectCode.Length == 3)
                {
                    AspectInstance = _dataService.GetAspect(_aspectCode);
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
                if (_instance != null && _instance.ID != 0)
                    _instance.Load();

                _deleteBatch.RaiseCanExecuteChanged();

                _samplesList = _instance.GetSamples();

                _selectedTrialArea = _trialAreaList.FirstOrDefault(tra => tra.ID == _instance?.TrialAreaID);
                

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
                RaisePropertyChanged("SelectedProject");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("SelectedTrialArea");
            }
        }

        public DelegateCommand CancelEditCommand
        {
            get { return _cancelEdit; }
        }

        private bool CanDelete => _instance != null
                                    && _principal.IsInRole(UserRoleNames.BatchEdit)
                                    && !_instance.HasTests;

        public IEnumerable<Colour> ColourList
        {
            get
            {
                if (_colourList == null)
                    _colourList = _dataService.GetColours();
                return _colourList;
            }
        }

        public DelegateCommand DeleteBatchCommand
        {
            get { return _deleteBatch; }
        }

        public DelegateCommand<Sample> DeleteSampleCommand => _deleteSample;

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
                _cancelEdit.RaiseCanExecuteChanged();
                _refresh.RaiseCanExecuteChanged();
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }
        
        public IEnumerable<ExternalConstruction> ExternalConstructionList
        {
            get
            {
                if (_externalConstructionList == null)
                    _externalConstructionList = _dataService.GetExternalConstructions();

                return _externalConstructionList;
            }
        }

        public IEnumerable<ExternalReport> ExternalReportList => _instance?.GetExternalReports();
        
        public string LineCode
        {
            get { return _lineCode; }
            set
            {
                _lineCode = value;

                if (_lineCode.Length == 3)
                {
                    LineInstance = _dataService.GetMaterialLine(_lineCode);
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

        public string Number
        {
            get
            {
                return _instance?.Number;
            }
        }

        public DelegateCommand OpenExternalReportCommand
        {
            get { return _openExternalReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public IEnumerable<Project> ProjectList
        {
            get => (_projectList != null) ? _projectList : _projectList = _dataService.GetProjects();
            private set => _projectList = value; 
        }

        public IEnumerable<Sample> Samples
        {
            get { return _samplesList; }
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

        public string RecipeCode
        {
            get { return _recipeCode; }
            set
            {
                _recipeCode = value;

                if (_recipeCode.Length == 4)
                {
                    RecipeInstance = _dataService.GetRecipe(_recipeCode);
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

        public DelegateCommand RefreshCommand
        {
            get { return _refresh; }
        }

        public IEnumerable<Report> ReportList
        {
            get { return _instance?.Reports; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }
        
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

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            {
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
            }
        }

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedExternalReport; }
            set 
            {
                _selectedExternalReport = value;
                _openExternalReport.RaiseCanExecuteChanged();
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

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<TrialArea> TrialAreaList
        {
            get { return _trialAreaList; }
        }

        public string TypeCode
        {
            get { return _typeCode; }
            set
            {
                _typeCode = value;

                if (_typeCode.Length == 4)
                {
                    TypeInstance = _dataService.GetMaterialType(_typeCode);
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
    }
}
