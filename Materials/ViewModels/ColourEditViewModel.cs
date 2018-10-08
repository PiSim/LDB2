using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Materials.ViewModels
{
    public class ColourEditViewModel : BindableBase
    {
        #region Fields

        private Colour _colourInstance;
        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private Batch _selectedBatch;
        private Recipe _selectedRecipe;
        IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public ColourEditViewModel(IEventAggregator eventAggregator,
                                    IDataService<LabDbEntities> labDbData) : base()
        {
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;

            OpenBatchCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedBatch != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_colourInstance));
                    EditMode = false;
                },
                () => EditMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode && System.Threading.Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialEdit));
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<Batch> BatchList => _colourInstance.GetBatches();

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

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand OpenBatchCommand { get; }

        public IEnumerable<Recipe> RecipeList
        {
            get
            {
                if (_colourInstance == null)
                    return new List<Recipe>();
                else
                    return _colourInstance.GetRecipes();
            }
        }

        public DelegateCommand SaveCommand { get; }

        public Batch SelectedBatch
        {
            get
            {
                return _selectedBatch;
            }

            set
            {
                _selectedBatch = value;
                OpenBatchCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand StartEditCommand { get; }

        #endregion Properties
    }
}