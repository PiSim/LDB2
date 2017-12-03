using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
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
    public class ColourMainViewModel : BindableBase
    {
        private Colour _selectedColour;
        private DBPrincipal _principal;
        private DelegateCommand _createColour,
                                _deleteColour;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;

        public ColourMainViewModel(DBPrincipal principal,
                                    EventAggregator eventAggregator,
                                    IDataService dataService) : base()
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _principal = principal;

            _createColour = new DelegateCommand(
                () =>
                {
                    Controls.Views.StringInputDialog newColourDialog = new Controls.Views.StringInputDialog();
                    newColourDialog.Message = "Inserire il nome per il nuovo colore:";
                    if (newColourDialog.ShowDialog() == true)
                    {

                        if (DBManager.Services.MaterialService.GetColour(newColourDialog.InputString) != null
                            || newColourDialog.InputString.Length > 45)
                            return;

                        Colour newColour = new Colour()
                        {
                            Code = "",
                            Name = newColourDialog.InputString
                        };

                        newColour.Create();
                        RaisePropertyChanged("ColourList");
                    }
                },
                () => _principal.IsInRole(UserRoleNames.MaterialEdit));

            _deleteColour = new DelegateCommand(
                () =>
                {
                    _selectedColour.Delete();
                    RaisePropertyChanged("ColourList");
                },
                () => _selectedColour != null 
                    && _principal.IsInRole(UserRoleNames.MaterialAdmin));
        }   

        public string ColourEditRegionName
        {
            get { return RegionNames.ColourEditRegion; }
        }

        public IEnumerable<Colour> ColourList => _dataService.GetColours();

        public DelegateCommand CreateColourCommand
        {
            get { return _createColour; }
        }

        public DelegateCommand DeleteColourCommand
        {
            get { return _deleteColour; }
        }

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
                _deleteColour.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.ColourEdit,
                                                            _selectedColour,
                                                            RegionNames.ColourEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
