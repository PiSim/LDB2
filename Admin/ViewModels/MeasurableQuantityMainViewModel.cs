using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
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
    public class MeasurableQuantityMainViewModel : BindableBase
    {
        private DelegateCommand _deleteQuantity,
                                _newQuantity;
        private EventAggregator _eventAggregator;
        private MeasurableQuantity _selectedMeasurableQuantity;

        public MeasurableQuantityMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _deleteQuantity = new DelegateCommand(
                () =>
                {
                    _selectedMeasurableQuantity.Delete();
                });

            _newQuantity = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<MeasurableQuantityCreationRequested>()
                                    .Publish();
                });

            _eventAggregator.GetEvent<MeasurableQuantityCreated>()
                            .Subscribe(
                            quantity =>
                            {
                                RaisePropertyChanged("MeasurableQuantityList");
                            });
        }

        public IEnumerable<MeasurableQuantity> MeasurableQuantityList
        {
            get { return InstrumentService.GetMeasurableQuantities(); }
        }

        public DelegateCommand DeleteQuantityCommand
        {
            get { return _deleteQuantity; }
        }

        public DelegateCommand NewMeasurableQuantityCommand
        {
            get { return _newQuantity; }
        }

        public MeasurableQuantity SelectedMeasurableQuantity
        {
            get { return _selectedMeasurableQuantity; }
            set
            {
                _selectedMeasurableQuantity = value;
            }
        }
    }
}
