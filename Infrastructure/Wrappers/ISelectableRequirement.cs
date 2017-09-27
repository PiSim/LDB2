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
        double Duration { get; }
        bool IsSelected { get; set; }
        Requirement RequirementInstance { get; }
    }
}
