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
    
    public partial class MethodMeasurement
    {
        public int ID { get; private set; }
        public int MethodID { get; private set; }
        public string Name { get; set; }
        public string UM { get; set; }
    
        public virtual Method Method { get; set; }
    }
}