//------------------------------------------------------------------------------
// <auto-generated>
//     Codice generato da un modello.
//
//     Le modifiche manuali a questo file potrebbero causare un comportamento imprevisto dell'applicazione.
//     Se il codice viene rigenerato, le modifiche manuali al file verranno sovrascritte.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExternalReportBatchMapping
    {
        public int ID { get; set; }
        public int ExternalReportID { get; set; }
        public int BatchID { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual ExternalReport ExternalReports { get; set; }
    }
}
