﻿using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{
    public class BatchCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private Aspect _aspectInstance;
        private Batch _batchInstance;
        private bool _projectPickEnabled;
        private Colour _selectedColour;
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private ExternalConstruction _selectedConstruction;
        private IEnumerable<Colour> _colourList;
        private IEnumerable<ExternalConstruction> _constructionList;
        private IEnumerable<Project> _projectList;
        private Material _materialInstance;
        private MaterialLine _lineInstance;
        private MaterialType _typeInstance;
        private Project _selectedProject;
        private Recipe _recipeInstance;
        private string _aspectCode,
                        _batchNumber,
                        _lineCode,
                        _notes,
                        _recipeCode,
                        _typeCode;
        private TrialArea _selectedTrialArea;

        public BatchCreationDialogViewModel() : base()
        {
            _colourList = MaterialService.GetColours();
            _constructionList = MaterialService.GetExternalConstructions();
            _projectList = ProjectService.GetProjects();

            _notes = "";

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    if (AspectInstance == null)
                    {
                        AspectInstance = new Aspect()
                        {
                            Code = _aspectCode
                        };

                        AspectInstance.Create();
                    };

                    if (LineInstance == null)
                    {
                        LineInstance = new MaterialLine()
                        {
                            Code = _lineCode
                        };

                        LineInstance.Create();
                    }

                    if (RecipeInstance == null)
                    {
                        RecipeInstance = new Recipe()
                        {
                            Code = _recipeCode
                        };

                        if (_selectedColour != null)
                            RecipeInstance.ColourID = _selectedColour.ID;

                        RecipeInstance.Create();
                    }

                    Material tempMaterial = MaterialService.GetMaterial(_typeInstance,
                                                                        _lineInstance,
                                                                        _aspectInstance,
                                                                        _recipeInstance);

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
                            tempMaterial.Update();
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

                        tempMaterial.Create();
                    }

                    _batchInstance = new Batch()
                    {
                        BasicReportDone = false,
                        FirstSampleArrived = false,
                        MaterialID = tempMaterial.ID,
                        Notes = _notes,
                        Number = _batchNumber
                    };

                    if (_selectedTrialArea != null)
                        _batchInstance.TrialAreaID = _selectedTrialArea.ID;

                    _batchInstance.Create();

                    parentDialog.DialogResult = true;
                },
                parentDialog => !HasErrors);

            BatchNumber = "";
            TypeCode = "";
            LineCode = "";
            AspectCode = "";
            RecipeCode = "";
        }

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
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion

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
                MaterialInstance = MaterialService.GetMaterial(TypeInstance,
                                                                LineInstance,
                                                                AspectInstance,
                                                                RecipeInstance);
            }
        }

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
                UpdateMaterial();
            }
        }

        public Batch BatchInstance
        {
            get { return _batchInstance; }
            private set
            {
                _batchInstance = value;
            }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;

                if (string.IsNullOrEmpty(_batchNumber))
                    BatchInstance = null;

                else
                    BatchInstance = MaterialService.GetBatch(_batchNumber);
                
                if (_batchInstance == null && 
                    _validationErrors.ContainsKey("BatchNumber"))
                {
                    _validationErrors.Remove("BatchNumber");
                    RaiseErrorsChanged("BatchNumber");
                }

                else
                {
                    _validationErrors["BatchNumber"] = new List<string>() { "Il batch " + _batchNumber + "  esiste già" };
                    RaiseErrorsChanged("BatchNumber");
                }
            }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public IEnumerable<Colour> ColourList
        {
            get { return _colourList; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<ExternalConstruction> ConstructionList
        {
            get { return _constructionList; }
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
                    SelectedConstruction = _constructionList.FirstOrDefault(con => con.ID == _materialInstance.ExternalConstructionID);
                    SelectedProject = _projectList.FirstOrDefault(prj => prj.ID == _materialInstance.ProjectID);
                    
                    ProjectPickEnabled = (SelectedProject == null);
                }
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
            }
        }

        public IEnumerable<Project> ProjectList
        {
            get { return ProjectService.GetProjects(); }
        }
        
        public bool ProjectPickEnabled
        {
            get { return _projectPickEnabled; }
            set
            {
                _projectPickEnabled = value;
                RaisePropertyChanged("ProjectPickEnabled");
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

                if (_recipeInstance != null)
                    SelectedColour = _colourList.FirstOrDefault(col => col.ID == _recipeInstance.ColourID);
                
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
            }
        }

        public TrialArea SelectedTrialArea
        {
            get { return _selectedTrialArea; }
            set
            {
                _selectedTrialArea = value;
            }
        }

        public IEnumerable<TrialArea> TrialAreaList
        {
            get { return MaterialService.GetTrialAreas(); }
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
                UpdateMaterial();
            }
        }

    }
}
