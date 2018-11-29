using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class InstrumentProperty
    {
        public InstrumentProperty()
        {
            Name = "";
            Value = 0;
            IsCalibrationProperty = false;
        }

        public int ID { get; set; }

        public int InstrumentID { get; set; }

        public string Name { get; set; }
        public int Value { get; set; }

        public bool IsCalibrationProperty { get; set; }

        public Instrument Instrument { get; set; }
    }
}
