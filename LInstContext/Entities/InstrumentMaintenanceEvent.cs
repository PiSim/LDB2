using System;

namespace LInst
{
    public class InstrumentMaintenanceEvent
    {
        #region Constructors

        public InstrumentMaintenanceEvent()
        {
            Description = "";
        }

        #endregion Constructors

        #region Properties

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int ID { get; set; }

        public Instrument Instrument { get; set; }
        public int InstrumentID { get; set; }
        public Person Tech { get; set; }
        public int? TechID { get; set; }

        #endregion Properties
    }
}