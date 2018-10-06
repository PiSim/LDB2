using System;
using System.Collections.Generic;
namespace LabDbContextCore
{
    public partial class PersonRoleMapping
    {
        public int ID { get; set; }
        public int personID { get; set; }
        public int roleID { get; set; }
        public bool IsSelected { get; set; }

        public virtual Person Person { get; set; }
        public virtual PersonRole Role { get; set; }
    }
}