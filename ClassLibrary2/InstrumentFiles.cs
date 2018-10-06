using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class InstrumentFiles
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int InstrumentID { get; set; }

        public virtual Instrument Instrument { get; set; }
    }
}