using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class UserRole
    {
        public UserRole()
        {
            Name = "";
            Description = "";
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserRoleMapping> UserMappings { get; }
    }
}
