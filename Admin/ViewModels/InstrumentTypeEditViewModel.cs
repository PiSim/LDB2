using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class InstrumentTypeEditViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private InstrumentType _instrumentTypeInstance;

        private MeasurableQuantity _selectedAssociated,
                                    _selectedUnassociated;

        #endregion Fields

        #region Constructors

        public InstrumentTypeEditViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            AssociateMeasurableCommand = new DelegateCommand(
                () =>
                {
                    _instrumentTypeInstance.AddMeasurableQuantityAssociation(_selectedUnassociated);
                    RaisePropertyChanged("AssociatedMeasurableQuantityList");
                    RaisePropertyChanged("UnassociatedMeasurableQuantityList");
                },
                () => _instrumentTypeInstance != null
                    && _selectedUnassociated != null);

            UnassociateMeasurableCommand = new DelegateCommand(
                () =>
                {
                    _instrumentTypeInstance.RemoveMeasurableQuantityAssociation(_selectedAssociated);
                    RaisePropertyChanged("AssociatedMeasurableQuantityList");
                    RaisePropertyChanged("UnassociatedMeasurableQuantityList");
                },
                () => _instrumentTypeInstance != null
                    && _selectedAssociated != null);
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<MeasurableQuantity> AssociatedMeasurableQuantityList => _instrumentTypeInstance.GetAssociatedMeasurableQuantities();
        public DelegateCommand AssociateMeasurableCommand { get; }

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
                UnassociateMeasurableCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedAssociated");
            }
        }

        public MeasurableQuantity SelectedUnassociated
        {
            get { return _selectedUnassociated; }
            set
            {
                _selectedUnassociated = value;
                AssociateMeasurableCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedUnassociated");
            }
        }

        public IEnumerable<MeasurableQuantity> UnassociatedMeasurableQuantityList => _instrumentTypeInstance.GetUnassociatedMeasurableQuantities();
        public DelegateCommand UnassociateMeasurableCommand { get; }

        #endregion Properties
    }
}