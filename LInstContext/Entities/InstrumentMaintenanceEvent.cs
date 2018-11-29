using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class InstrumentMaintenanceEvent
    {
        public InstrumentMaintenanceEvent()
        {
            Description = "";
        }

        public int ID { get; set; }

        public int InstrumentID { get; set; }
        public int? TechID { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Instrument Instrument { get; set; }
        public Person Tech { get; set; }
    }
}
