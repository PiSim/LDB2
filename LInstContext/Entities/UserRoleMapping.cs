using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class UserRoleMapping
    {
        public int ID { get; set; }

        public int UserID { get; set; }
        public int UserRoleID { get; set; }

        public User User { get; set; }
        public UserRole UserRole { get; set; }

        public bool IsSelected { get; set; }
    }
}
