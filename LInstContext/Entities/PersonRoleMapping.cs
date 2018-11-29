using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class PersonRoleMapping
    {
        public int ID { get; set; }
        
        public int PersonID { get; set; }
        public int RoleID { get; set; }

        public Person Person { get; set; }
        public PersonRole Role { get; set; }

        public bool IsSelected { get; set; }
    }
}
