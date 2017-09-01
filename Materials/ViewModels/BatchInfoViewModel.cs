using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
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
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private ExternalConstruction _selectedExternalConstruction;
        private ExternalReport _selectedExternalReport;
        private IEnumerable<Colour> _colourList;
        private IEnumerable<ExternalConstruction> _externalConstructionList;
        private IEnumerable<TrialArea> _trialAreaList; 
        private List<SamplesWrapper> _samplesList;
        private Material _materialInstance;
        private MaterialLine _lineInstance;
        private Recipe _recipeInstance;
        private Report _selectedReport;
        private string _aspectCode,
                        _lineCode,
                        _recipeCode,
                        _typeCode;
        private MaterialType _typeInstance;
        private TrialArea _selectedTrialArea;

        public BatchInfoViewModel(DBPrincipal principal,
                                EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;
            _editMode = false;

            _colourList = MaterialService.GetColours();
            _externalConstructionList = MaterialService.GetExternalConstructions();
            _trialAreaList = MaterialService.GetTrialAreas();

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                });

            _cancelEdit = new DelegateCommand(
                () =>
                {
                    if (_instance == null)
                        BatchInstance = null;
                    else
                        BatchInstance = MaterialService.GetBatch(_instance.ID);
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
                () => _principal.IsInRole(UserRoleNames.BatchAdmin));

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
                    BatchInstance = MaterialService.GetBatch(_instance.ID);
                },
                () => !EditMode);

            _save = new DelegateCommand(
                () =>
                {
                    if (_materialInstance == null)
                    {
                        _materialInstance = new Material();

                        _materialInstance.TypeID = _typeInstance.ID;

                        // If line exists sets the lineID in the material,
                        // otherwise creates new line instance

                        if (_lineInstance == null)
                        {
                            _lineInstance = new MaterialLine()
                            {
                                Code = LineCode                                
                            };

                            _materialInstance.MaterialLine = _lineInstance;
                        }

                        else
                            _materialInstance.LineID = _lineInstance.ID;

                        // If aspect exists sets the aspectID in the material,
                        // otherwise creates new aspect instance


                        if (_aspectInstance == null)
                        {
                            _aspectInstance = new Aspect()
                            {
                                Code = AspectCode,
                                Name = ""
                            };

                            _materialInstance.Aspect = _aspectInstance;
                        }

                        else
                            _materialInstance.AspectID = _aspectInstance.ID;

                        // If recipe exists sets the recipeID in the material,
                        // otherwise creates new recipe instance and sets the colour if one is selected

                        if (_recipeInstance == null)
                        {
                            _recipeInstance = new Recipe()
                            {
                                Code = RecipeCode,
                                ColourID = _selectedColour?.ID
                            };

                            _materialInstance.Recipe = _recipeInstance;
                        }

                        else
                            _materialInstance.RecipeID = _recipeInstance.ID;

                        // Sets the external construction if one is selected

                        if (_selectedExternalConstruction != null)
                            _materialInstance.ExternalConstructionID = _selectedExternalConstruction.ID;

                        // Creates the material entry and any new sub-entries

                        _materialInstance.Create();
                    }

                    // If material exists and a construction is selected
                    // sets the construction

                    else if (_selectedExternalConstruction != null)
                    {
                        _materialInstance.ExternalConstructionID = _selectedExternalConstruction.ID;
                        _materialInstance.Update();
                    }

                    // If a color was selected
                    // sets the colorID 

                    if (_selectedColour != null 
                        && _recipeInstance.ColourID != _selectedColour.ID)
                    {
                        _recipeInstance.ColourID = _selectedColour.ID;
                        _recipeInstance.Update();
                    }

                    // Updates the material FK if it's different from the one stored in the DB

                    if (_materialInstance.ID != _instance.MaterialID)
                        _instance.MaterialID = _materialInstance.ID;

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
        }

        #region Service method for internal use

        private void CheckMaterial()
        {
            // If all subEntities are not null checks to see if a corresponding material exists

            if (_typeInstance != null &&
                _lineInstance != null &&
                _aspectInstance != null &&
                _recipeInstance != null)
                MaterialInstance = MaterialService.GetMaterial(_typeInstance,
                                                                _lineInstance,
                                                                _aspectInstance,
                                                                _recipeInstance);
            else
                MaterialInstance = null;
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
                    AspectInstance = MaterialService.GetAspect(_aspectCode);
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
                
                _samplesList = _instance.GetSamples()?.Select(smp => new SamplesWrapper(smp)).ToList();

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

                SelectedExternalConstruction = _externalConstructionList.FirstOrDefault(excon => excon.ID == _materialInstance?.ExternalConstructionID);
                SelectedColour = _colourList.FirstOrDefault(col => col.ID == _recipeInstance?.ColourID);

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
                RaisePropertyChanged("Project");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("SelectedTrialArea");
            }
        }

        public DelegateCommand CancelEditCommand
        {
            get { return _cancelEdit; }
        }

        public IEnumerable<Colour> ColourList
        {
            get { return _colourList; }
        }

        public DelegateCommand DeleteBatchCommand
        {
            get { return _deleteBatch; }
        }

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
            get { return _externalConstructionList; }
        }

        public IEnumerable<ExternalReport> ExternalReportList 
        {
            get 
            { 
                return _instance?.ExternalReports; 
            }
        }
        
        public string LineCode
        {
            get { return _lineCode; }
            set
            {
                _lineCode = value;

                if (_lineCode.Length == 3)
                {
                    LineInstance = MaterialService.GetLine(_lineCode);
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

        public Project Project
        {
            get
            {
                return _instance?.Material?.Project;
            }
        }

        public List<SamplesWrapper> Samples
        {
            get { return _samplesList; }
        }

        public string RecipeCode
        {
            get { return _recipeCode; }
            set
            {
                _recipeCode = value;

                if (_recipeCode.Length == 4)
                {
                    RecipeInstance = MaterialService.GetRecipe(_recipeCode);
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
            get { return _selectedExternalConstruction; }
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
                    TypeInstance = MaterialService.GetMaterialType(_typeCode);
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
