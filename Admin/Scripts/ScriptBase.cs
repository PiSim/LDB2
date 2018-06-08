using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin
{
    public class ScriptBase
    {
        protected string _name,
                        _description;

        public string Name => _name;
        public string Description => _description;

        public virtual void Run()
        {

        }
    }
}
