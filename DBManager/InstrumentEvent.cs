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
    
    public partial class InstrumentEvent
    {
        public int ID { get; set; }
        public int instrumentID { get; set; }
        public int organizationID { get; set; }
        public System.DateTime Date { get; set; }
        public int EventTypeID { get; set; }
    }
}