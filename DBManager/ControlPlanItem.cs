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
    
    public partial class ControlPlanItem
    {
        public int ControlPlanID { get; set; }
        public int RequirementID { get; set; }
        public bool IsSelected { get; set; }
        public int ID { get; set; }
    
        public virtual ControlPlan ControlPlan { get; set; }
        public virtual Requirement Requirement { get; set; }
    }
}
