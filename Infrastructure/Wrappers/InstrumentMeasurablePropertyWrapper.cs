using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Wrappers
{
    public class InstrumentMeasurablePropertyWrapper : BindableBase
    {
        #region Constructors

        public InstrumentMeasurablePropertyWrapper(InstrumentMeasurableProperty instance) : base()
        {
            IsSelected = false;
            IsModified = false;
            PropertyInstance = instance;
            UMList = PropertyInstance.GetMeasurementUnits();
        }

        #endregion Constructors

        #region Properties

        public double CalibrationLowerRangeValue
        {
            get { return PropertyInstance.CalibrationRangeLowerLimit; }
            set
            {
                PropertyInstance.CalibrationRangeLowerLimit = value;
                IsModified = true;
            }
        }

        public double CalibrationUpperRangeValue
        {
            get { return PropertyInstance.CalibrationRangeUpperLimit; }
            set
            {
                PropertyInstance.CalibrationRangeUpperLimit = value;
                IsModified = true;
            }
        }

        public float Division
        {
            get { return PropertyInstance.Resolution; }
            set
            {
                PropertyInstance.Resolution = value;
                IsModified = true;
            }
        }

        public bool IsModified { get; private set; }

        public bool IsSelected { get; set; }

        public double LowerRangeValue
        {
            get { return PropertyInstance.RangeLowerLimit; }
            set
            {
                PropertyInstance.RangeLowerLimit = value;
                IsModified = true;
            }
        }

        public string Name => PropertyInstance.MeasurableQuantity.Name;

        public InstrumentMeasurableProperty PropertyInstance { get; }

        public float TargetUncertainty
        {
            get { return PropertyInstance.TargetUncertainty; }
            set
            {
                PropertyInstance.TargetUncertainty = value;
                IsModified = true;
            }
        }

        public MeasurementUnit UM
        {
            get { return UMList.First(um => um.ID == PropertyInstance.UnitID); }
            set
            {
                PropertyInstance.UnitID = value.ID;
                IsModified = true;
            }
        }

        public IEnumerable<MeasurementUnit> UMList { get; }

        public double UpperRangeValue
        {
            get { return PropertyInstance.RangeUpperLimit; }
            set
            {
                PropertyInstance.RangeUpperLimit = value;
                IsModified = true;
            }
        }

        #endregion Properties
    }
}