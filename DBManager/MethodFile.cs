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
    
    public partial class MethodFile
    {
        public int ID { get; private set; }
        public int IssueID { get; private set; }
        public string Path { get; set; }
        public string Description { get; set; }
    
        public virtual MethodIssue MethodIssue { get; set; }
    }
}
