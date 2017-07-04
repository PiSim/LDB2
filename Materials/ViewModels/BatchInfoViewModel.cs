using Controls.Views;
using DBManager;
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
                                _openExternalReport, 
                                _openReport,
                                _refresh,
                                _save,
                                _startEdit;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private ExternalConstruction _externalConstructionInstance;
        private ExternalReport _selectedExternalReport;
        private IEnumerable<Colour> _colourList;
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
                    BatchInstance = MaterialService.GetBatch(_instance.ID);
                },
                () => EditMode);
               
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
                    _instance.Update();
                    EditMode = false;
                },
                () => EditMode && !HasErrors);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode);
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
                EditMode = false;

                _instance = value;
                _instance.Load();

                _samplesList = _instance.GetSamples().Select(smp => new SamplesWrapper(smp)).ToList();

                _selectedTrialArea = _trialAreaList.FirstOrDefault(tra => tra.ID == _instance.TrialAreaID);
                

                _typeInstance = _instance.Material.MaterialType;
                _typeCode = _typeInstance.Code;

                _lineInstance = _instance.Material.MaterialLine;
                _lineCode = _lineInstance.Code;

                _aspectInstance = _instance.Material.Aspect;
                _aspectCode = _aspectInstance.Code;

                _recipeInstance = _instance.Material.Recipe;
                _recipeCode = _recipeInstance.Code;

                _materialInstance = _instance.Material;

                _externalConstructionInstance = _materialInstance.ExternalConstruction;

                SelectedExternalReport = null;
                SelectedReport = null;

                RaisePropertyChanged("TypeInstance");
                RaisePropertyChanged("TypeCode");
                RaisePropertyChanged("LineCode");
                RaisePropertyChanged("AspectInstance");
                RaisePropertyChanged("AspectCode");
                RaisePropertyChanged("RecipeCode");
                RaisePropertyChanged("SelectedColour");

                RaisePropertyChanged("ExternalConstructionName");

                RaisePropertyChanged("ExternalReportList");
                RaisePropertyChanged("Samples");
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
        
        public bool ColourPickEnabled
        {
            get { return EditMode; }
        }

        public string ConstructionName
        {
            get { return _externalConstructionInstance?.Name; }
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

            }
        }

        public DelegateCommand RefreshCommand
        {
            get { return _refresh; }
        }

        public IEnumerable<Report> ReportList
        {
            get { return _instance.Reports; }
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
