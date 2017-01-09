using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Navigation
{
    public interface IModuleNavigationTag
    {
        string ViewName { get; }
    }
}
