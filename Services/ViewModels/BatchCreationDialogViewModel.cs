using DBManager;
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
        private Colour _selectedColour;
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<Colour> _colourList;
        private MaterialLine _lineInstance;
        private MaterialType _typeInstance;
        private Project _selectedProject;
        private Recipe _recipeInstance;
        private string _aspectCode,
                        _lineCode,
                        _recipeCode,
                        _trialScope,
                        _typeCode;
        private TrialArea _selectedTrialArea;

        public BatchCreationDialogViewModel() : base()
        {
            _colourList = MaterialService.GetColours();

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });
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

        public string LineCode
        {
            get { return _lineCode; }
            set
            {
                _lineCode = value;
            }
        }

        public MaterialLine LineInstance
        {
            get { return _lineInstance; }
            set
            {
                _lineInstance = value;
                RaisePropertyChanged("LineInstance");
            }
        }

        public IEnumerable<Project> ProjectList
        {
            get { return ProjectService.GetProjects(); }
        }
        
        public Recipe RecipeInstance
        {
            get { return _recipeInstance; }
            set
            {
                _recipeInstance = value;

                if (_recipeInstance != null)
                    SelectedColour = _colourList.FirstOrDefault(col => col.ID == _recipeInstance.ColourID);
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

        public string TrialScope
        {
            get { return _trialScope; }
            set
            {
                _trialScope = value;
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
            }
        }

    }
}
