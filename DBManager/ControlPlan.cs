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
    
    public partial class ControlPlan
    {
        public long ID { get; set; }
        public string deletion_flag { get; set; }
        public string name { get; set; }
        public long specificationID { get; set; }
        public string test_plan { get; set; }
    
        public virtual Specification Specification { get; set; }
    }
}
