using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Materials.ViewModels
{
    public class ColourMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Colour _selectedColour;

        #endregion Fields

        #region Constructors

        public ColourMainViewModel(IEventAggregator eventAggregator,
                                    IDataService<LabDbEntities> labDbdata) : base()
        {
            _labDbData = labDbdata;
            _eventAggregator = eventAggregator;

            CreateColourCommand = new DelegateCommand(
                () =>
                {
                    Controls.Views.StringInputDialog newColourDialog = new Controls.Views.StringInputDialog();
                    newColourDialog.Message = "Inserire il nome per il nuovo colore:";
                    if (newColourDialog.ShowDialog() == true)
                    {
                        if (_labDbData.RunQuery(new ColorsQuery()).Any(col => col.Name == newColourDialog.InputString)
                            || newColourDialog.InputString.Length > 45)
                            return;

                        Colour newColour = new Colour()
                        {
                            Code = "",
                            Name = newColourDialog.InputString
                        };

                        newColour.Create();

                        _eventAggregator.GetEvent<ColorChanged>()
                                        .Publish(new EntityChangedToken(newColour,
                                                                        EntityChangedToken.EntityChangedAction.Created));
                    }
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialEdit));

            DeleteColourCommand = new DelegateCommand(
                () =>
                {
                    _selectedColour.Delete(); _eventAggregator.GetEvent<ColorChanged>()
                                         .Publish(new EntityChangedToken(_selectedColour,
                                                                         EntityChangedToken.EntityChangedAction.Deleted));

                    SelectedColour = null;
                },
                () => _selectedColour != null
                    && Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialAdmin));

            #region Events

            _eventAggregator.GetEvent<ColorChanged>()
                            .Subscribe(ect =>
                            {
                                RaisePropertyChanged("ColourList");
                            });

            #endregion Events
        }

        #endregion Constructors

        #region Properties

        public string ColourEditRegionName => RegionNames.ColourEditRegion;

        public IEnumerable<Colour> ColourList => _labDbData.RunQuery(new ColorsQuery())
                                                                .ToList();

        public DelegateCommand CreateColourCommand { get; }

        public DelegateCommand DeleteColourCommand { get; }

        public Colour SelectedColour
        {
            get
            {
                return _selectedColour;
            }

            set
            {
                _selectedColour = value;
                RaisePropertyChanged("SelectedColour");
                DeleteColourCommand.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.ColourEdit,
                                                            _selectedColour,
                                                            RegionNames.ColourEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

        #endregion Properties
    }
}