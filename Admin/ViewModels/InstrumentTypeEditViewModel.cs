using DBManager;
using DBManager.EntityExtensions;
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
    public class InstrumentTypeEditViewModel : BindableBase
    {
        private DelegateCommand _associateMeasurable,
                                _unassociateMeasurable;
        private EventAggregator _eventAggregator;
        private InstrumentType _instrumentTypeInstance;
        private MeasurableQuantity _selectedAssociated,
                                    _selectedUnassociated;

        public InstrumentTypeEditViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _associateMeasurable = new DelegateCommand(
                () =>
                {
                    _instrumentTypeInstance.AddMeasurableQuantityAssociation(_selectedUnassociated);
                    RaisePropertyChanged("AssociatedMeasurableQuantityList");
                    RaisePropertyChanged("UnassociatedMeasurableQuantityList");
                },
                () => _instrumentTypeInstance != null
                    && _selectedUnassociated != null);

            _unassociateMeasurable = new DelegateCommand(
                () =>
                {
                    _instrumentTypeInstance.RemoveMeasurableQuantityAssociation(_selectedAssociated);
                    RaisePropertyChanged("AssociatedMeasurableQuantityList");
                    RaisePropertyChanged("UnassociatedMeasurableQuantityList");
                },
                () => _instrumentTypeInstance != null
                    && _selectedAssociated != null);
        }

        public DelegateCommand AssociateMeasurableCommand
        {
            get { return _associateMeasurable; }
        }

        public IEnumerable<MeasurableQuantity> AssociatedMeasurableQuantityList
        {
            get { return _instrumentTypeInstance.GetAssociatedMeasurableQuantities(); }
        }

        public InstrumentType InstrumentTypeInstance
        {
            get { return _instrumentTypeInstance; }
            set
            {
                _instrumentTypeInstance = value;
                SelectedAssociated = null;
                SelectedUnassociated = null;
                RaisePropertyChanged("AssociatedMeasurableQuantityList");
                RaisePropertyChanged("UnassociatedMeasurableQuantityList");
            }
        }

        public MeasurableQuantity SelectedAssociated
        {
            get { return _selectedAssociated; }
            set
            {
                _selectedAssociated = value;
                _unassociateMeasurable.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedAssociated");
            }
        }

        public MeasurableQuantity SelectedUnassociated
        {
            get { return _selectedUnassociated; }
            set
            {
                _selectedUnassociated = value;
                _associateMeasurable.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedUnassociated");
            }
        }

        public DelegateCommand UnassociateMeasurableCommand
        {
            get { return _unassociateMeasurable; }
        }

        public IEnumerable<MeasurableQuantity> UnassociatedMeasurableQuantityList
        {
            get { return _instrumentTypeInstance.GetUnassociatedMeasurableQuantities(); }
        }
    }
}
