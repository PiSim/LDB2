using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
   public  class StandardListViewModel : BindableBase
    {
        private IDataService _dataService;
        private IEventAggregator _eventAggregator;
        private Std _selectedStandard;

        public StandardListViewModel(IDataService dataService,
                                    IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            /// Event Subscriptions

            _eventAggregator.GetEvent<StandardChanged>()
                            .Subscribe(
                token =>
                {
                    SelectedStandard = null;
                    OnPropertyChanged("StandardList");
                });

        }

        public Std SelectedStandard
        {
            get => _selectedStandard;
            set
            {
                _selectedStandard = value;

                NavigationToken token = new NavigationToken(SpecificationViewNames.StandardEdit,
                                                            _selectedStandard,
                                                            StandardEditRegionName);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        public string StandardEditRegionName => RegionNames.StandardEditRegion;

        public IEnumerable<Std> StandardList => _dataService.GetStandards();

    }


}
