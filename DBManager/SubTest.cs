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
    
    public partial class SubTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubTest()
        {
            this.Result = "";
        }
    
        public int ID { get; set; }
        public string Result { get; set; }
        public int TestID { get; private set; }
        public string Name { get; set; }
        public string RequiredValue { get; set; }
        public string UM { get; set; }
        public Nullable<int> SubRequiremntID { get; set; }
        public Nullable<int> SubMethodID { get; set; }
        public Nullable<int> Position { get; set; }
    
        public virtual Test Test { get; set; }
        public virtual SubRequirement SubRequirement { get; set; }
        public virtual SubMethod SubMethod { get; set; }
    }
}
