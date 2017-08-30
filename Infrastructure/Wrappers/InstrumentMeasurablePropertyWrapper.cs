using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class InstrumentMeasurablePropertyWrapper : BindableBase
    {
        private bool _isSelected;
        private InstrumentMeasurableProperty _instance;

        public InstrumentMeasurablePropertyWrapper(InstrumentMeasurableProperty instance) : base()
        {
            _isSelected = false;
            _instance = instance;
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public bool IsUnderControl
        {
            get { return _instance.IsUnderControl; }
        }

        public string CalibrationDueDate
        {
            get
            {
                if (!_instance.CalibrationDue.HasValue)
                    return "Mai";

                return _instance.CalibrationDue.Value.ToString("d");
            }
        }
        public string Name
        {
            get { return _instance.MeasurableQuantity.Name; }
        }

        public InstrumentMeasurableProperty PropertyInstance
        {
            get { return _instance; }
        }


    }
}
