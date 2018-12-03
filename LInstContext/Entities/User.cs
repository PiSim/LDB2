﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class User
    {
        public User()
        {
            FullName = "";
            UserName = "";
            HashedPassword = "";

            RoleMappings = new HashSet<UserRoleMapping>();
        }


        public int ID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        public int PersonID { get; set; }

        public string HashedPassword { get; set; }

        public ICollection<UserRoleMapping> RoleMappings { get; set; }

        public Person Person { get; set; }

    }
}
