using DBManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public interface ISelectableRequirement : ITestItem
    {
        bool IsSelected { get; set; }
        Requirement RequirementInstance { get; }
    }
}
