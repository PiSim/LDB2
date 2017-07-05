using DBManager;
using DBManager.EntityExtensions;
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

namespace Materials.ViewModels
{
    public class ColourEditViewModel :BindableBase
    {
        private Batch _selectedBatch;
        private bool _editMode;
        private Colour _colourInstance;
        private DBPrincipal _principal;
        private DelegateCommand _openBatch,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private Recipe _selectedRecipe;

        public ColourEditViewModel(DBPrincipal principal,
                                    EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _principal = principal;

            _openBatch = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedBatch != null);

            _save = new DelegateCommand(
                () =>
                {
                    _colourInstance.Update();
                    EditMode = false;
                },
                () => EditMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode && _principal.IsInRole(UserRoleNames.MaterialEdit));
        }

        public IEnumerable<Batch> BatchList
        {
            get
            {
                return _colourInstance.GetBatches();
            }
        }

        public Colour ColourInstance
        {
            get { return _colourInstance; }
            set
            {
                EditMode = false;

                _colourInstance = value;
                
                RaisePropertyChanged("ColourInstance");
                RaisePropertyChanged("ColourName");
                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("RecipeList");
            }
        }

        public string ColourName
        {
            get
            {
                if (_colourInstance == null)
                    return null;

                else
                    return _colourInstance.Name;
            }
            set
            {
                _colourInstance.Name = value;
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public List<Recipe> RecipeList
        {
            get
            {
                if (_colourInstance == null)
                    return new List<Recipe>();

                else
                    return new List<Recipe>(_colourInstance.Recipes);
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public Batch SelectedBatch
        {
            get
            {
                return _selectedBatch;
            }

            set
            {
                _selectedBatch = value;
                _openBatch.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedBatch");
            }
        }

        public Recipe SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
