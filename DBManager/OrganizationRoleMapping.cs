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
    
    public partial class OrganizationRoleMapping
    {
        public int ID { get; set; }
        public int OrganizationID { get; set; }
        public int roleID { get; set; }
    
        public virtual Organization Organization { get; set; }
        public virtual OrganizationRole Roles { get; set; }
    }
}
