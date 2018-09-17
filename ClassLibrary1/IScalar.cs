using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IScalar<out T>
    {
        T Execute(DBContext context);
    }
}
