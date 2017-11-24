using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public interface ITestItem
    {
        string PropertyName { get; }
        string MethodName { get; }
        IEnumerable SubItems { get; }
        double? WorkHours { get; }
    }
}
