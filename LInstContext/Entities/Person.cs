﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class Person
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<PersonRoleMapping> RoleMappings { get; }
    }
}
