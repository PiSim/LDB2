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
        public string Requirement { get; set; }
        public string UM { get; set; }
    
        public virtual Test Test { get; set; }
    }
}
