using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class UserRoleMapping
    {
        public int ID { get; private set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public bool IsSelected { get; set; }

        public virtual UserRole UserRole { get; set; }
        public virtual User User { get; set; }
    }
}