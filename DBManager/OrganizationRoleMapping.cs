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
    
    public partial class OrganizationRoleMapping
    {
        public int ID { get; set; }
        public int OrganizationID { get; set; }
        public int roleID { get; set; }
        public bool IsSelected { get; set; }
    
        public virtual Organization Organization { get; set; }
        public virtual OrganizationRole Role { get; set; }
    }
}
