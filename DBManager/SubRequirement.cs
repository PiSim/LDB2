//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubRequirement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubRequirement()
        {
            this.RequiredValue = "";
        }
    
        public int ID { get; private set; }
        public string RequiredValue { get; set; }
        public int RequirementID { get; private set; }
        public int SubMethodID { get; set; }
    
        public virtual Requirement Requirement { get; set; }
        public virtual SubMethod SubMethod { get; set; }
    }
}
