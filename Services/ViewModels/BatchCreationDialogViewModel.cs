using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{
    public class BatchCreationDialogViewModel : BindableBase
    {
        private Aspect _aspectInstance;
        private Batch _batchInstance;
        private bool _colourPickEnabled,
                    _constructionPickEnabled,
                    _projectPickEnabled;
        private Colour _selectedColour;
        private DelegateCommand<Window> _cancel, _confirm;
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
                        _recipeCode,
                        _trialScopeText,
                        _typeCode;
        private TrialArea _selectedTrialArea;
        private TrialScope _selectedTrialScope;

        public BatchCreationDialogViewModel() : base()
        {
            _colourList = MaterialService.GetColours();
            _colourPickEnabled = true;
            _constructionList = MaterialService.GetExternalConstructions();
            _projectList = ProjectService.GetProjects();

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
                        Number = _batchNumber
                    };

                    _batchInstance.Create();

                    parentDialog.DialogResult = true;
                });
        }

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
                AspectInstance = MaterialService.GetAspect(_aspectCode);
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
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
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

        public bool ColourPickEnabled
        {
            get { return _colourPickEnabled; }
            set
            {
                _colourPickEnabled = value;
                RaisePropertyChanged("ColourPickEnabled");
            }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<ExternalConstruction> ConstructionList
        {
            get { return _constructionList; }
        }

        public bool ConstructionPickEnabled
        {
            get { return _constructionPickEnabled; }
            set
            {
                _constructionPickEnabled = value;
                RaisePropertyChanged("ConstructionPickEnabled");
            }
        }

        public string LineCode
        {
            get { return _lineCode; }
            set
            {
                _lineCode = value;
                LineInstance = MaterialService.GetLine(_lineCode);
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

                    ConstructionPickEnabled = (SelectedConstruction == null);
                    ProjectPickEnabled = (SelectedProject == null);
                }
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
                RecipeInstance = MaterialService.GetRecipe(_recipeCode);
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

                ColourPickEnabled = (SelectedColour == null);
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

        public TrialScope SelectedTrialScope
        {
            get { return _selectedTrialScope; }
        }

        public IEnumerable<TrialArea> TrialAreaList
        {
            get { return MaterialService.GetTrialAreas(); }
        }

        public string TrialScopeText
        {
            get { return _trialScopeText; }
            set
            {
                _trialScopeText = value;
            }
        }

        public string TypeCode
        {
            get { return _typeCode; }
            set
            {
                _typeCode = value;
                TypeInstance = MaterialService.GetMaterialType(_typeCode);
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
