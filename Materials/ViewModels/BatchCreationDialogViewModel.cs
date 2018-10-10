using DataAccess;
using Infrastructure.Commands;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Materials.Queries;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Materials.ViewModels
{
    public class BatchCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private string _aspectCode,
                        _batchNumber,
                        _lineCode, _recipeCode,
                        _typeCode;

        private Aspect _aspectInstance;
        private IDataService<LabDbEntities> _labDbData;
        private MaterialLine _lineInstance;
        private Material _materialInstance;
        private Recipe _recipeInstance;
        private Colour _selectedColour;
        private ExternalConstruction _selectedConstruction;
        private Project _selectedProject;
        private MaterialType _typeInstance;

        #endregion Fields

        #region Constructors

        public BatchCreationDialogViewModel(IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            ColourList = _labDbData.RunQuery(new ColorsQuery()).ToList();
            ConstructionList = _labDbData.RunQuery(new ConstructionsQuery()).ToList();
            DoNotTest = false;
            ProjectList = _labDbData.RunQuery(new ProjectsQuery()).ToList();

            Notes = "";

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    if (AspectInstance == null)
                    {
                        AspectInstance = new Aspect()
                        {
                            Code = _aspectCode
                        };

                        _labDbData.Execute(new InsertEntityCommand(AspectInstance));
                    };

                    if (LineInstance == null)
                    {
                        LineInstance = new MaterialLine()
                        {
                            Code = _lineCode
                        };

                        _labDbData.Execute(new InsertEntityCommand(LineInstance));
                    }

                    if (RecipeInstance == null)
                    {
                        RecipeInstance = new Recipe()
                        {
                            Code = _recipeCode
                        };

                        if (_selectedColour != null)
                            RecipeInstance.ColourID = _selectedColour.ID;

                        _labDbData.Execute(new InsertEntityCommand(RecipeInstance));
                    }
                    else if (_selectedColour != null && RecipeInstance.ColourID != _selectedColour.ID)
                    {
                        _recipeInstance.ColourID = _selectedColour.ID;
                        _labDbData.Execute(new UpdateEntityCommand(_recipeInstance));
                    }

                    Material tempMaterial = _labDbData.RunQuery(new MaterialQuery()
                    {
                        AspectID = _aspectInstance.ID,
                        MaterialLineID = _lineInstance.ID,
                        MaterialTypeID = _typeInstance.ID,
                        RecipeID = _recipeInstance.ID
                    });

                    if (tempMaterial != null)
                    {
                        bool requiresUpdate = false;

                        if (_selectedConstruction != null
                            && tempMaterial.ExternalConstructionID != _selectedConstruction.ID)
                        {
                            tempMaterial.ExternalConstructionID = _selectedConstruction.ID;
                            requiresUpdate = true;
                        }

                        if (_selectedProject != null
                            && tempMaterial.ProjectID != _selectedProject.ID)
                        {
                            tempMaterial.ProjectID = _selectedProject.ID;
                            requiresUpdate = true;
                        }

                        if (requiresUpdate)
                            _labDbData.Execute(new UpdateEntityCommand(tempMaterial));
                    }

                    if (tempMaterial == null)
                    {
                        tempMaterial = new Material()
                        {
                            AspectID = AspectInstance.ID,
                            LineID = LineInstance.ID,
                            RecipeID = RecipeInstance.ID,
                            TypeID = TypeInstance.ID
                        };

                        if (_selectedConstruction != null)
                            tempMaterial.ExternalConstructionID = _selectedConstruction.ID;

                        if (_selectedProject != null)
                            tempMaterial.ProjectID = _selectedProject.ID;

                        _labDbData.Execute(new InsertEntityCommand(tempMaterial));
                    }

                    BatchInstance = new Batch()
                    {
                        DoNotTest = DoNotTest,
                        FirstSampleArrived = false,
                        MaterialID = tempMaterial.ID,
                        ArchiveStock = 0,
                        LongTermStock = 0,
                        Notes = Notes,
                        Number = _batchNumber
                    };

                    if (SelectedTrialArea != null)
                        BatchInstance.TrialAreaID = SelectedTrialArea.ID;
                    
                    _labDbData.Execute(new InsertEntityCommand(BatchInstance));

                    parentDialog.DialogResult = true;
                },
                parentDialog => !HasErrors);

            BatchNumber = "";
            TypeCode = "";
            LineCode = "";
            AspectCode = "";
            RecipeCode = "";
        }

        #endregion Constructors

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

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
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public string AspectCode
        {
            get { return _aspectCode; }
            set
            {
                _aspectCode = value;

                if (_aspectCode.Length == 3)
                {
                    AspectInstance = _labDbData.RunQuery(new AspectQuery() { AspectCode = _aspectCode });
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
                UpdateMaterial();
            }
        }

        public Batch BatchInstance { get; private set; }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;

                if (string.IsNullOrEmpty(_batchNumber))
                {
                    BatchInstance = null;
                    _validationErrors["BatchNumber"] = new List<string>() { "'" + _batchNumber + "' non è un batch valido" };
                }
                else
                {
                    BatchInstance = _labDbData.RunQuery(new BatchQuery() { Number = _batchNumber });

                    if (BatchInstance == null)
                    {
                        if (_validationErrors.ContainsKey("BatchNumber"))
                            _validationErrors.Remove("BatchNumber");
                    }
                    else
                        _validationErrors["BatchNumber"] = new List<string>() { "Il batch " + _batchNumber + "  esiste già" };
                }

                RaiseErrorsChanged("BatchNumber");
            }
        }

        public DelegateCommand<Window> CancelCommand { get; }

        public IEnumerable<Colour> ColourList { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IEnumerable<ExternalConstruction> ConstructionList { get; }

        public bool DoNotTest { get; set; }

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
                RaisePropertyChanged("LineInstance");
                UpdateMaterial();
            }
        }

        public Material MaterialInstance
        {
            get { return _materialInstance; }
            set
            {
                _materialInstance = value;
                if (_materialInstance != null)
                {
                    SelectedConstruction = ConstructionList.FirstOrDefault(con => con.ID == _materialInstance.ExternalConstructionID);
                    SelectedProject = ProjectList.FirstOrDefault(prj => prj.ID == _materialInstance.ProjectID);
                }
            }
        }

        public string Notes { get; set; }

        public IEnumerable<Project> ProjectList { get; }

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

                if (_recipeInstance != null && _recipeInstance.ColourID != null)
                    SelectedColour = ColourList.FirstOrDefault(col => col.ID == _recipeInstance.ColourID);

                UpdateMaterial();
            }
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

        public ExternalConstruction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set
            {
                _selectedConstruction = value;
                RaisePropertyChanged("SelectedConstruction");
            }
        }

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                RaisePropertyChanged("SelectedProject");
            }
        }

        public TrialArea SelectedTrialArea { get; set; }

        public IEnumerable<TrialArea> TrialAreaList => _labDbData.RunQuery(new TrialAreasQuery()).ToList();

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
                UpdateMaterial();
            }
        }

        #endregion Properties

        #region Methods

        private void UpdateMaterial()
        {
            if (AspectInstance == null
                || LineInstance == null
                || RecipeInstance == null
                || TypeInstance == null)
            {
                MaterialInstance = null;
            }
            else
            {
                MaterialInstance = _labDbData.RunQuery(new MaterialQuery()
                {
                    AspectID = _aspectInstance.ID,
                    MaterialLineID = _lineInstance.ID,
                    MaterialTypeID = _typeInstance.ID,
                    RecipeID = _recipeInstance.ID
                });
            }
        }

        #endregion Methods
    }
}