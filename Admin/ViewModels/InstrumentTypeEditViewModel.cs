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
        private DelegateCommand _assignMeasurable,
                                _unassignMeasurable;
        private EventAggregator _eventAggregator;
        private InstrumentType _instrumentTypeInstance;
        private MeasurableQuantity _selectedAssigned,
                                    _selectedUnassigned;

        public InstrumentTypeEditViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _assignMeasurable = new DelegateCommand(
                () =>
                {

                });

            _unassignMeasurable = new DelegateCommand(
                () =>
                {

                });
        }

        public DelegateCommand AssignMeasurableCommand
        {
            get { return _assignMeasurable; }
        }

        public IEnumerable<MeasurableQuantity> AssociatedMeasurablesList
        {
            get { return _instrumentTypeInstance.GetAssociatedMeasurableQuantities(); }
        }

        public InstrumentType InstrumentTypeInstance
        {
            get { return _instrumentTypeInstance; }
            set
            {
                _instrumentTypeInstance = value;
                SelectedAssigned = null;
                SelectedUnassigned = null;
            }
        }

        public MeasurableQuantity SelectedAssigned
        {
            get { return _selectedAssigned; }
            set
            {
                _selectedAssigned = value;
                RaisePropertyChanged("AssociatedMeasurablesList");
                RaisePropertyChanged("SelectedAssigned");
                RaisePropertyChanged("UnassociatedMeasurablesList");
            }
        }

        public MeasurableQuantity SelectedUnassigned
        {
            get { return _selectedUnassigned; }
            set
            {
                _selectedUnassigned = value;
                RaisePropertyChanged("SelectedUnassigned");
            }
        }

        public DelegateCommand UnassignMeasurableCommand
        {
            get { return _unassignMeasurable; }
        }

        public IEnumerable<MeasurableQuantity> UnassociatedMeasurablesList
        {
            get { return _instrumentTypeInstance.GetUnassociatedMeasurableQuantities(); }
        }
    }
}
