//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabDbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class InstrumentFiles
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public int InstrumentID { get; set; }
    
        public virtual Instrument Instrument { get; set; }
    }
}
