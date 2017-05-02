using DBManager;
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
        private Colour _colourInstance;
        private DBEntities _entities;
        private DelegateCommand _openBatch;
        private EventAggregator _eventAggregator;
        private Recipe _selectedRecipe;

        public ColourEditViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;

            _openBatch = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedBatch != null);
        }

        public List<Batch> BatchList
        {
            get
            {
                if (_colourInstance == null)
                    return new List<Batch>();
                else
                    return new List<Batch>(_entities.Batches.Where(btc => btc.Material.Recipe.Colour.ID == _colourInstance.ID));
            }
        }

        public Colour ColourInstance
        {
            get { return _colourInstance; }
            set
            {
                if (value != null)
                    _colourInstance = _entities.Colours.First(clr => clr.ID == value.ID);
                
                else
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

        public Batch SelectedBatch
        {
            get
            {
                return _selectedBatch;
            }

            set
            {
                _selectedBatch = value;
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
    }
}
