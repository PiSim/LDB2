using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class SubRequirement
    {
        public string Name => SubMethod?.Name;

        public string UM => SubMethod?.UM;
    }
}
