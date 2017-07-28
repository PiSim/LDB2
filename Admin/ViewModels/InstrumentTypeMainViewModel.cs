using DBManager;
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

namespace Admin.ViewModels
{
    public class InstrumentTypeMainViewModel : BindableBase
    {
        private DelegateCommand _newInstrumentType;
        private EventAggregator _eventAggregator;
        private InstrumentType _selectedInstrumentType;

        public InstrumentTypeMainViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _newInstrumentType = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<InstrumentTypeCreationRequested>()
                                    .Publish();
                });

            _eventAggregator.GetEvent<InstrumentTypeCreated>()
                            .Subscribe(
                            instrumentType =>
                            {
                                RaisePropertyChanged("InstrumentTypeList");
                            });
        }

        public IEnumerable<InstrumentType> InstrumentTypeList
        {
            get { return InstrumentService.GetInstrumentTypes(); }
        }

        public string InstrumentTypeManagementEditRegionName
        {
            get { return RegionNames.InstrumentTypeManagementEditRegion; }
        }

        public DelegateCommand NewInstrumentTypeCommand
        {
            get { return _newInstrumentType; }
        }

        public InstrumentType SelectedInstrumentType
        {
            get { return _selectedInstrumentType; }
            set
            {
                _selectedInstrumentType = value;

                NavigationToken token = new NavigationToken(AdminViewNames.InstrumentTypeEditView,
                                                            _selectedInstrumentType,
                                                            RegionNames.InstrumentTypeManagementEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

    }
}
