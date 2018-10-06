using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class InstrumentMaintenanceEvent
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
        public int InstrumentID { get; set; }
        public Nullable<int> PersonID { get; set; }

        public virtual Instrument Instrument { get; set; }
        public virtual Person Person { get; set; }
    }
}