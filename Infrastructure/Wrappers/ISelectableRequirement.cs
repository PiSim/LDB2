using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public interface ISelectableRequirement
    {
        bool IsSelected { get; }
        Requirement RequirementInstance { get; }
    }
}
