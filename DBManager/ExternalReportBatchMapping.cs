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
    
    public partial class ExternalReportBatchMapping
    {
        public int ID { get; set; }
        public int ExternalReportID { get; set; }
        public int BatchID { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual ExternalReport ExternalReports { get; set; }
    }
}
