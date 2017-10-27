using DBManager;
using DBManager.EntityExtensions;
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
        private bool _isModified,
                    _isSelected;
        private IEnumerable<MeasurementUnit> _umList;
        private InstrumentMeasurableProperty _instance;

        public InstrumentMeasurablePropertyWrapper(InstrumentMeasurableProperty instance) : base()
        {
            _isSelected = false;
            _isModified = false;
            _instance = instance;
            _umList = _instance.GetMeasurementUnits();
        }

        public double CalibrationLowerRangeValue
        {
            get { return _instance.CalibrationRangeLowerLimit; }
            set
            {
                _instance.CalibrationRangeLowerLimit = value;
                _isModified = true;
            }
        }

        public double CalibrationUpperRangeValue
        {
            get { return _instance.CalibrationRangeUpperLimit; }
            set
            {
                _instance.CalibrationRangeUpperLimit = value;
                _isModified = true;
            }
        }

        public float Division
        {
            get { return _instance.Resolution; }
            set
            {
                _instance.Resolution = value;
                _isModified = true;
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public double LowerRangeValue
        {
            get { return _instance.RangeLowerLimit; }
            set
            {
                _instance.RangeLowerLimit = value;
                _isModified = true;
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

        public float TargetUncertainty
        {
            get { return _instance.TargetUncertainty; }
            set
            {
                _instance.TargetUncertainty = value;
                _isModified = true;
            }
        }
        
        public MeasurementUnit UM
        {
            get { return _umList.First(um => um.ID == _instance.UnitID); }
            set
            {
                _instance.UnitID = value.ID;
                _isModified = true;
            }
        }

        public IEnumerable<MeasurementUnit> UMList
        {
            get { return _umList; }
        }

        public double UpperRangeValue
        {
            get { return _instance.RangeUpperLimit; }
            set
            {
                _instance.RangeUpperLimit = value;
                _isModified = true;
            }
        }
    }
}
