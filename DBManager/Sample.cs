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
    
    public partial class Sample
    {
        public int ID { get; set; }
        public int BatchID { get; set; }
        public string Code { get; set; }
        public System.DateTime Date { get; set; }
        public int personID { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Person LogAuthor { get; set; }
    }
}
